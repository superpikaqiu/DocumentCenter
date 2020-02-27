using AutoMapper;
using Newtonsoft.Json;
using DocumentCenter.Controllers.FileManage.Dto;
using DocumentCenter.DB;
using DocumentCenter.Domain.Enum;
using DocumentCenter.Domain.Helper;
using DocumentCenter.Dto.Document;
using DocumentCenter.Dto.FileManage;
using DocumentCenter.Dto.User;
using DocumentCenter.Models;
using DocumentCenter.Service.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace DocumentCenter.Service.Implement
{
    public class FileManageService : IFileManageService
    {
        private readonly DocumentDbContext _context = new DocumentDbContext();
        private readonly IDocumentService documentService = new DocumentService();
        private readonly IUserService userService = new UserService();

        private readonly string ONLYOFFICE_HOST = System.Configuration.ConfigurationManager.AppSettings["OnlyOffice.DocumentServer"];
        private readonly string PMS_HOST = System.Configuration.ConfigurationManager.AppSettings["PMSHost"];
        private readonly string CDPM_HOST = System.Configuration.ConfigurationManager.AppSettings["CdpmHost"];
        //PMS系统文件同步接口
        private readonly string UPDATE_FILE_SYNC_PMS = "/initiate/filesync";
        //PMS系统评论提取接口
        private readonly string Opinion_SYNC_PMS = "/v0.1/api/result/approval/opinion/controlOpinionByResultId";
        //生产系统评论提取接口
        private readonly string Opinion_SYNC_CDPM = "/liteproject/webservice/productwebservice.asmx/SaveDocumentServerOpinionList";
        //文件类型转换接口
        private readonly string CONVERSION_API = "/ConvertService.ashx";

        public bool ExternalRefreshFile(ExternalRefreshFileInput input)
        {
            var documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.ExternalID == input.ID);

            if (documentInfo == null)
            {
                return false;
            }

            DocumentHistory documentHistory = _context.DocumentHistories.FirstOrDefault(a => a.DocumentInfoId == documentInfo.Id && a.Version == documentInfo.CurrentVersion);

            documentInfo.Key = Guid.NewGuid().ToString();
            documentHistory.Key = documentInfo.Key;
            if (!string.IsNullOrEmpty(input.NewID))
            {
                documentInfo.ExternalID = input.NewID;
            }

            documentService.StoreDocumentFile(documentInfo, input.FilePath);
            _context.SaveChanges();

            return true;
        }

        public int ExternalSaveFile(ExternalSaveFileInput input)
        {
            CreateDocumentInput createDocumentInput = Mapper.Map<CreateDocumentInput>(input);
            createDocumentInput.ExternalFlag = true;

            DocumentInfo info = documentService.CreateDocument(createDocumentInput, input.ExternalFilePath);
            return info.Id;
        }

        public ExternalSaveFileByPmsOutput ExternalSaveFileByPms(ExternalSaveFileInput input)
        {
            UserDto userDto = new UserDto
            {
                UserID = input.CreateUserID,
                UserName = input.CreateUserName
            };

            userService.SaveUser(userDto);

            DocumentInfo info = _context.DocumentInfos.FirstOrDefault(a => a.ExternalFilePath == input.ExternalFilePath);

            if (info != null)
            {
                info.ExtendData = input.ExtendData;
                _context.SaveChanges();
                return new ExternalSaveFileByPmsOutput
                {
                    Id = info.Id,
                };
            }

            input.ExternalCallbackUrl = PMS_HOST + UPDATE_FILE_SYNC_PMS;
            input.SystemType = SystemType.pms.ToString();
            var id = ExternalSaveFile(input);

            return new ExternalSaveFileByPmsOutput
            {
                Id = id,
            };
        }

        public int ExternalSaveFileByYF(ExternalSaveFileInput input)
        {
            //之前有编码过的需要解码，解决乱码问题
            input.FileName = HttpUtility.UrlDecode(input.FileName);
            input.CreateUserName = HttpUtility.UrlDecode(input.CreateUserName);
            input.ExternalFilePath = HttpUtility.UrlDecode(input.ExternalFilePath);
            input.SystemType = SystemType.cdpm.ToString();

            UserDto userDto = new UserDto
            {
                UserID = input.CreateUserID,
                UserName = input.CreateUserName
            };

            userService.SaveUser(userDto);

            DocumentInfo info = _context.DocumentInfos.FirstOrDefault(a => a.ExternalID == input.ExternalID);

            if (!string.IsNullOrEmpty(input.ExternalID) && info != null)
            {
                return info.Id;
            }

            return ExternalSaveFile(input);
        }

        public void FileSyncCallback(List<FileSyncResultDto> fileSyncResultDtos)
        {
            var ids = fileSyncResultDtos.Select(a => a.ModifyFileId).ToArray();
            var documentList = _context.DocumentInfos.Where(a => ids.Contains(a.Id)).ToList();
            foreach (var fileSyncResult in fileSyncResultDtos)
            {
                var document = documentList.FirstOrDefault(a => a.Id == fileSyncResult.ModifyFileId);
                if (document != null && fileSyncResult.Result == 0)
                {
                    document.LastUpdateTime = DateTime.Now;
                }
            }

            _context.SaveChanges();
        }

        public GetFileHistoryOutput GetFileHistory(int id)
        {
            var histories = _context.DocumentHistories.Where(a => a.DocumentInfoId == id).ToList();
            List<DocumentHistoryDto> historyList = histories.Select(a => Mapper.Map<DocumentHistoryDto>(a)).ToList();
            int curVersion = _context.DocumentInfos.FirstOrDefault(a => a.Id == id).CurrentVersion;

            return new GetFileHistoryOutput
            {
                CurVersion = curVersion,
                Items = historyList
            };
        }

        public DocumentInfoDto GetFileInfo(int id)
        {
            var documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.Id == id);

            //通知远程服务器开始编辑文件
            if (documentInfo.ExternalFlag && documentInfo.ExternalStartEditUrl != null)
            {
                var data = new
                {
                    id = documentInfo.ExternalID
                };

                WebRequestHelper.PostData(documentInfo.ExternalStartEditUrl, data);
            }

            return Mapper.Map<DocumentInfoDto>(documentInfo);
        }



        public string GetFileKey(int id)
        {
            var documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.Id == id);
            return documentInfo?.Key;
        }

        public List<SyncDocumentDto> GetSyncDocumentList()
        {
            var origin = "http://" + HttpContext.Current.Request.ServerVariables.Get("HTTP_HOST").ToString();

            var updateTime = DateTime.Now;
            var documentList = _context.DocumentInfos.Where(a => a.ExternalFlag == true && a.IsEditing == true
                && (a.LastUpdateTime == null || a.LastUpdateTime <= updateTime)).ToList();

            return documentList.Select(a => new SyncDocumentDto
            {
                modifyFileId = a.Id,
                modifyFileUrl = origin + a.RelativePath,
                originalFileUrl = a.ExternalFilePath
            }).ToList();
        }

        public bool SaveFile(SaveFileInput input)
        {
            try
            {
                var fileData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(input.Data);
                int status = (int)fileData["status"];
                //状态4处理手动保存后关闭文档的响应，让远程可以回调
                if (status == 4)
                {
                    var key = fileData["key"].ToString();

                    DocumentInfo documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.Key == key);
                    //处理远程回调
                    if (documentInfo.ExternalFlag)
                    {
                        ExternalCallback(documentInfo, input.ServerOrigin, false);
                    }

                }
                else if (status == 2)
                {
                    var key = fileData["key"].ToString();
                    var userList = (ArrayList)fileData["users"];
                    string userId = userList.Count > 0 ? userList[0].ToString() : null;

                    var user = _context.Users.FirstOrDefault(a => a.UserID == userId);
                    DocumentInfo documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.Key == key);
                    DocumentHistory documentHistory = _context.DocumentHistories.FirstOrDefault(a => a.DocumentInfoId == documentInfo.Id && a.Version == documentInfo.CurrentVersion);

                    

                    documentInfo.Key = Guid.NewGuid().ToString();
                    documentInfo.ModifyTime = DateTime.Now;
                    documentInfo.ModifyUserId = user?.UserID;
                    documentInfo.ModifyUserName = user?.UserName;

                    //不使用history的key，因为可能修改文件后没有产生新版本
                    documentHistory.Key = documentInfo.Key;

                    documentService.StoreDocumentFile(documentInfo, (string)fileData["url"]);
                    //txt文件需要做转换处理
                    string convertPath = ConvertFile(documentInfo);
                    if (!string.IsNullOrWhiteSpace(convertPath))
                    {
                        documentService.StoreDocumentFile(documentInfo, convertPath);
                    }
                    
                    _context.SaveChanges();
                    //处理远程回调
                    if (documentInfo.ExternalFlag)
                    {
                        ExternalCallback(documentInfo, input.ServerOrigin, true);
                    }
                }
                else if (status == 6)
                {
                    var key = fileData["key"].ToString();
                    var historyDic = (Dictionary<string, object>)fileData["history"];
                    var serverVersion = historyDic["serverVersion"].ToString();
                    var changes = new JavaScriptSerializer().Serialize(historyDic["changes"]);
                    var changesUrl = fileData["changesurl"].ToString();
                    var userList = (ArrayList)fileData["users"];
                    string userId = userList.Count > 0 ? userList[0].ToString() : null;

                    //更新文档信息
                    DocumentInfo documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.Key == key);
                    DocumentHistory documentHistory = _context.DocumentHistories.Where(a => a.DocumentInfoId == documentInfo.Id)
                        .OrderByDescending(a => a.Version).FirstOrDefault();

                    bool newVersion = documentHistory.Key == documentInfo.Key;

                    documentInfo.CurrentVersion = newVersion ? documentHistory.Version + 1 : documentHistory.Version;
                    _context.SaveChanges();

                    //存储新文档
                    var user = _context.Users.FirstOrDefault(a => a.UserID == userId);
                    string userName = user?.UserName;

                    CreateHistoryInput createHistoryInput = new CreateHistoryInput
                    {
                        DocumentInfoId = documentInfo.Id,
                        Version = documentInfo.CurrentVersion,
                        PreviousVersion = documentInfo.CurrentVersion - 1,
                        ServerVersion = serverVersion,
                        Key = newVersion ? Guid.NewGuid().ToString() : documentHistory.Key,
                        Changes = changes,
                        UserId = userId,
                        UserName = userName,
                        AutoSave = false,
                        CreateTime = DateTime.Now,
                        FileType = documentInfo.FileType,
                        HistoryUrl = (string)fileData["url"],
                        ChangesUrl = changesUrl
                    };

                    documentService.CreateHistory(createHistoryInput);
                }


                return true;
            }
            catch
            {
                return true;
            }

        }

        public void SaveOpinionList(SaveOpinionInput input)
        {
            try
            {
                var documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.Id == input.DocumentInfoId);

                if (documentInfo.SystemType == SystemType.cdpm.ToString())
                {
                    SaveOpinionListToCdpm(documentInfo, input.OpinionList);
                }
                else if (documentInfo.SystemType == SystemType.pms.ToString())
                {
                    SaveOpinionListToPms(documentInfo, input.OpinionList);
                }
            }
            catch
            {

            }
        }

        public void UpdateEditingUrl(UpdateEditingUrlInput input)
        {
            DocumentInfo documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.Id == input.ID);

            if (documentInfo == null)
            {
                return;
            }

            documentService.StoreDocumentFile(documentInfo, input.Url);
            documentInfo.IsEditing = true;
            _context.SaveChanges();
        }

        public int UploadDesignVolumeProduct(UploadDesignVolumeProductInput input, Stream stream)
        {
            CreateDocumentInput createDocumentInput = new CreateDocumentInput
            {
                CreateUserID = input.UserID,
                CreateUserName = input.UserName,
                FileName = input.FileName,
                FileType = input.FileType,
                ExternalFlag = true
            };

            var documentInfo = documentService.CreateDocument(createDocumentInput, stream);
            return documentInfo.Id;
        }

        private void ExternalCallback(DocumentInfo documentInfo, string serverOrigin, bool saveFile)
        {
            object data = new
            {
                id = documentInfo.ExternalID,
                documentId = documentInfo.Id,
                filePath = serverOrigin + documentInfo.RelativePath,
                save = saveFile,
                modify_file_url = serverOrigin + documentInfo.RelativePath,
                original_file_url = documentInfo.ExternalFilePath
            };

            var result = WebRequestHelper.PostData(documentInfo.ExternalCallbackUrl, data);

            if (result == "0")
            {
                documentInfo.LastUpdateTime = DateTime.Now;
            }

            documentInfo.IsEditing = false;
            _context.SaveChanges();
        }

        private void SaveOpinionListToCdpm(DocumentInfo documentInfo, string opinionList)
        {
            //var url = CDPM_HOST + Opinion_SYNC_CDPM;
            var url = documentInfo.ExternalSaveOpinionUrl;

            var data = new
            {
                id = documentInfo.ExternalID,
                opinionList = opinionList
            };

            WebRequestHelper.PostData(url, data);
        }

        private void SaveOpinionListToPms(DocumentInfo documentInfo, string opinionList)
        {
            var url = PMS_HOST + Opinion_SYNC_PMS;

            var extendDataDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(documentInfo.ExtendData);
            var data = new OpinionPmsDto();
            data.result_id = extendDataDic.ContainsKey("result_id") ? extendDataDic["result_id"].ToString() : "";
            data.type = extendDataDic.ContainsKey("type") ? extendDataDic["type"].ToString() : "";
            data.opinion_list = new List<OpinionItemPmsDto>();

            var opinions = JsonConvert.DeserializeObject<List<OpinionDto>>(opinionList);
            foreach (var opinion in opinions)
            {
                var opinionTemp = new OpinionItemPmsDto();
                opinionTemp.opinion_content = opinion.Content;
                opinionTemp.create_uid = opinion.UserID;
                opinionTemp.reply_opinion_list = new List<OpinionReplyPmsDto>();
                data.opinion_list.Add(opinionTemp);

                foreach (var opinionReply in opinion.Replys)
                {
                    var opinionReplyTemp = new OpinionReplyPmsDto();
                    opinionReplyTemp.content = opinionReply.Content;
                    opinionReplyTemp.create_uid = opinionReply.UserID;
                    opinionTemp.reply_opinion_list.Add(opinionReplyTemp);
                }
            }

            Dictionary<string, string> headers = new Dictionary<string, string>();
            var token = extendDataDic.ContainsKey("token") ? extendDataDic["token"].ToString() : "";
            headers.Add("Authorization", "Bearer " + token);
            WebRequestHelper.PostData(url, data, headers);
        }

        private string ConvertFile(DocumentInfo documentInfo)
        {
            string url = null;
            var apiUrl = ONLYOFFICE_HOST + CONVERSION_API;
            var origin = "http://" + HttpContext.Current.Request.ServerVariables.Get("HTTP_HOST").ToString();
            if (documentInfo.FileType == "txt")
            {
                apiUrl += "?async=false&key=" + documentInfo.Key
                    + "&filetype=docx&outputtype=" + documentInfo.FileType
                    + "&url=" + origin + documentInfo.RelativePath;
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                string result = WebRequestHelper.GetData(apiUrl, headers);
                var resultDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                if (resultDic.ContainsKey("fileUrl"))
                {
                    url = resultDic["fileUrl"].ToString();
                }
            }

            return url;
        }
    }
}