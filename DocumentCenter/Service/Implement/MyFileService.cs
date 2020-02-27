using AutoMapper;
using Newtonsoft.Json;
using DocumentCenter.DB;
using DocumentCenter.Domain.Enum;
using DocumentCenter.Domain.Helper;
using DocumentCenter.Dto.Document;
using DocumentCenter.Dto.FileShare;
using DocumentCenter.Dto.MyFile;
using DocumentCenter.Dto.User;
using DocumentCenter.Models;
using DocumentCenter.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DocumentCenter.Service.Implement
{
    public class MyFileService : IMyFileService
    {
        private readonly DocumentDbContext _context = new DocumentDbContext();
        private readonly IDocumentService documentService = new DocumentService();
        private readonly IUserService userService = new UserService();
        private readonly IFileShareService fileShareService = new FileShareService();

        public MyFileDto GetFile(int id)
        {
            var personalFile = _context.PersonalFiles.FirstOrDefault(a => a.Id == id);
            var documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.Id == personalFile.DocumentID);
            var myFileDto = Mapper.Map<MyFileDto>(personalFile);
            myFileDto.UploadFileName = documentInfo.FileName;
            return myFileDto;
        }

        public GetFileListOutput GetFileList(GetFileListInput input)
        {
            var query = from file in _context.PersonalFiles
                        join document in _context.DocumentInfos on file.DocumentID equals document.Id
                        where file.CreateUserID == input.UserID
                        select new FileListDto
                        {
                            Id = file.Id,
                            DocumentID = file.DocumentID,
                            Size = document.Size,
                            FileName = document.FileName,
                            CreateUserName = file.CreateUserName,
                            ModifyUserName = file.ModifyUserName,
                            TempTime = file.ModifyTime,
                            Path = document.Path,
                            RelativePath = document.RelativePath
                        };

            if (!string.IsNullOrEmpty(input.FileName))
            {
                query = query.Where(a => a.FileName.Contains(input.FileName));
            }

            int count = query.Count();
            var list = query.OrderBy(a => a.Id).Skip(input.Limit * (input.Page - 1)).Take(input.Limit).ToList();
            foreach (var item in list)
            {
                item.ModifyTime = item.TempTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return new GetFileListOutput
            {
                Count = count,
                Items = list
            };
        }

        public bool RemoveFile(string id)
        {
            var ids = id.Split(',');

            foreach (var item in ids)
            {
                PersonalFile personalFile = _context.PersonalFiles.Find(int.Parse(item));

                if (personalFile != null)
                {
                    _context.PersonalFiles.Remove(personalFile);
                }
            }

            _context.SaveChanges();
            return true;
        }

        public void UpdateFileAfterEdit(int id)
        {
            var personalFile = _context.PersonalFiles.FirstOrDefault(a => a.Id == id);
            personalFile.ModifyUserID = HttpContext.Current.Session["UserID"].ToString();
            personalFile.ModifyUserName = HttpContext.Current.Session["UserName"].ToString();
            personalFile.ModifyTime = DateTime.Now;
            _context.SaveChanges();
        }

        public int Upload(UploadInput input)
        {
            var userId = HttpContext.Current.Session["UserID"].ToString();
            var userName = HttpContext.Current.Session["UserName"].ToString();

            UserDto userDto = new UserDto
            {
                UserID = userId,
                UserName = userName
            };

            userService.SaveUser(userDto);

            PersonalFile personalFile = _context.PersonalFiles.FirstOrDefault(a => a.Id == input.Id);

            DocumentInfo documentInfo = null;

            CreateDocumentInput createDocumentInput = new CreateDocumentInput
            {
                CreateUserID = userId,
                CreateUserName = userName,
                FileName = input.FileName + "." + input.FileType,
                FileType = input.FileType
            };

            //新建文件
            if (input.CreateType == CreateType.newFile.ToString() && (personalFile == null || input.FileType != personalFile.FileType))
            {
                MemoryStream stream = new MemoryStream();
                if (input.FileType == FileType.xlsx.ToString())
                {
                    stream = new MemoryStream(NPOIHelper.CreateExcelFile());
                }
                documentInfo = documentService.CreateDocument(createDocumentInput, stream);
            }
            else if (input.CreateType == CreateType.uploadFile.ToString() && (personalFile == null || HttpContext.Current.Request.Files.Count > 0))
            {
                if (HttpContext.Current.Request.Files.Count == 0)
                {
                    return -1;
                }

                var uploadFile = HttpContext.Current.Request.Files[0];
                createDocumentInput.FileName = uploadFile.FileName;
                createDocumentInput.FileType = createDocumentInput.FileName.Substring(createDocumentInput.FileName.LastIndexOf('.') + 1);
                input.FileName = createDocumentInput.FileName.Substring(0,createDocumentInput.FileName.LastIndexOf('.'));
                if(createDocumentInput.FileType == FileType.doc.ToString() || createDocumentInput.FileType == FileType.docx.ToString())
                {
                    input.FileType = FileType.docx.ToString();
                }
                else
                {
                    input.FileType = FileType.xlsx.ToString();
                }

                documentInfo = documentService.CreateDocument(createDocumentInput, uploadFile.InputStream);
            }

            if (personalFile == null)
            {
                var createTime = DateTime.Now;
                personalFile = new PersonalFile
                {
                    DocumentID = documentInfo.Id,
                    CreateType = input.CreateType,
                    FileName = input.FileName,
                    FileType = input.FileType,
                    CreateUserID = userId,
                    CreateUserName = userName,
                    CreateTime = createTime,
                    ModifyUserID = userId,
                    ModifyUserName = userName,
                    ModifyTime = createTime
                };
                _context.PersonalFiles.Add(personalFile);
            }
            else
            {
                //personalFile.CreateType = input.CreateType;
                personalFile.FileName = input.FileName;
                //personalFile.FileType = input.FileType;
                documentInfo = _context.DocumentInfos.FirstOrDefault(a => a.Id == personalFile.DocumentID);
                documentInfo.FileName = input.FileName + "." + personalFile.FileType;
            }

            _context.SaveChanges();

            var shareUserList = JsonConvert.DeserializeObject<List<FileUserDto>>(input.ShareUserList);
            fileShareService.AddShareUser(new AddShareUserInput
            {
                FileId = personalFile.Id,
                FileUserDtos = shareUserList
            });

            return personalFile.Id;
        }
    }
}