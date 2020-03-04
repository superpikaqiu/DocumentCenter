using AutoMapper;
using DocumentCenter.DB;
using DocumentCenter.Domain.Helper;
using DocumentCenter.Dto.Document;
using DocumentCenter.Models;
using DocumentCenter.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace DocumentCenter.Service.Implement
{
    public class DocumentService : IDocumentService
    {
        private readonly DocumentDbContext _context = new DocumentDbContext();
        private readonly string FilePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"];
        private readonly string HistoryPath = System.Configuration.ConfigurationManager.AppSettings["HistoryPath"];
        private readonly string ChangesPath = System.Configuration.ConfigurationManager.AppSettings["ChangesPath"];

        public DocumentInfo CreateDocument(CreateDocumentInput input, Stream stream)
        {
            var documentInfo = Mapper.Map<DocumentInfo>(input);
            StoreDocumentFile(documentInfo, stream);
            HandleTifFile(documentInfo.FileType, documentInfo.Path);

            documentInfo.Key = Guid.NewGuid().ToString();
            documentInfo.CreateTime = DateTime.Now;
            documentInfo.ModifyTime = documentInfo.CreateTime;
            documentInfo.ModifyUserId = documentInfo.CreateUserID;
            documentInfo.ModifyUserName = documentInfo.CreateUserName;
            documentInfo.CurrentVersion = 1;

            _context.DocumentInfos.Add(documentInfo);
            _context.SaveChanges();

            var historyPath = StoreHistoryFile(stream, documentInfo.FileType);
            DocumentHistory fileHistory = new DocumentHistory
            {
                DocumentInfoId = documentInfo.Id,
                Version = 1,
                PreviousVersion = -1,
                Key = documentInfo.Key,
                Path = historyPath,
                UserId = input.CreateUserID,
                UserName = input.CreateUserName,
                CreateTime = DateTime.Now
            };

            _context.DocumentHistories.Add(fileHistory);
            _context.SaveChanges();

            return documentInfo;
        }

        public DocumentInfo CreateDocument(CreateDocumentInput input, string url)
        {
            var req = WebRequest.CreateHttp(url);
            string authCookie = HttpContext.Current.Session["YFMIS3"] != null ? HttpContext.Current.Session["YFMIS3"].ToString() : "";
            req.Headers.Add(HttpRequestHeader.Cookie, "YFMIS3=" + authCookie);
            var stream = req.GetResponse().GetResponseStream();
            var documentInfo = CreateDocument(input, stream);
            stream.Close();
            return documentInfo;
        }

        public void CreateHistory(CreateHistoryInput input)
        {
            var historyPath = StoreHistoryFile(input.HistoryUrl, input.FileType);
            var changesPath = StoreChangesFile(input.ChangesUrl, input.FileType);

            var history = _context.DocumentHistories.FirstOrDefault(a => a.DocumentInfoId == input.DocumentInfoId && a.Version == input.Version);
            if (history == null)
            {
                history = Mapper.Map<DocumentHistory>(input);
                history.Path = historyPath;
                history.ChangesUrl = changesPath;

                _context.DocumentHistories.Add(history);
            }
            else
            {
                Mapper.Map(input, history);
                history.Path = historyPath;
                history.ChangesUrl = changesPath;
            }

            _context.SaveChanges();
        }

        private void HandleTifFile(string fileType, string path)
        {
            if (fileType == "tif")
            {
                var storePath = path.Substring(0, path.LastIndexOf('.')) + ".gif"; ;
                TIFHelper.TIFToGif(path, storePath);
            }
        }

        private long SaveFile(Stream stream, string storePath)
        {
            var buffer = new byte[4096];
            int readed;

            FileStream fileStream = new FileStream(storePath, FileMode.Create);

            while ((readed = stream.Read(buffer, 0, 4096)) != 0)
            {
                fileStream.Write(buffer, 0, readed);
            }

            var length = fileStream.Length;
            fileStream.Close();
            return length;
        }

        private long SaveFile(string url, string storePath)
        {
            long length = 0;
            var req = WebRequest.Create(url);
            using (var stream = req.GetResponse().GetResponseStream())
            {
                length = SaveFile(stream, storePath);
            }

            return length;
        }

        public void StoreDocumentFile(DocumentInfo documentInfo, Stream stream)
        {
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + documentInfo.FileType;
            string storePath = HttpContext.Current.Server.MapPath(FilePath);

            if (!Directory.Exists(storePath))
            {
                Directory.CreateDirectory(storePath);
            }

            storePath += newFileName;
            documentInfo.RelativePath = FilePath + newFileName;
            documentInfo.Path = storePath;

            var length = SaveFile(stream, storePath); ;
            documentInfo.Size = length;
        }

        public void StoreDocumentFile(DocumentInfo documentInfo, string url)
        {
            var req = WebRequest.Create(url);
            var stream = req.GetResponse().GetResponseStream();
            StoreDocumentFile(documentInfo, stream);
            stream.Close();
        }

        public string StoreHistoryFile(string url, string fileType)
        {
            var req = WebRequest.Create(url);
            var stream = req.GetResponse().GetResponseStream();
            var path = StoreHistoryFile(stream, fileType);
            stream.Close();
            return path;
        }

        public string StoreHistoryFile(Stream stream, string fileType)
        {
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + fileType;
            string storePath = HttpContext.Current.Server.MapPath(HistoryPath);
            if (!Directory.Exists(storePath))
            {
                Directory.CreateDirectory(storePath);
            }

            storePath += newFileName;
            SaveFile(stream, storePath);
            return storePath;
        }

        public string StoreChangesFile(string url, string fileType)
        {
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip";
            string storePath = HttpContext.Current.Server.MapPath(ChangesPath);
            if (!Directory.Exists(storePath))
            {
                Directory.CreateDirectory(storePath);
            }

            storePath += newFileName;
            SaveFile(url, storePath);
            return ChangesPath + newFileName;
        }

    }
}