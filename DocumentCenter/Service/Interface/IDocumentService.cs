using DocumentCenter.Dto.Document;
using DocumentCenter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DocumentCenter.Service.Interface
{
    public interface IDocumentService
    {
        DocumentInfo CreateDocument(CreateDocumentInput input, Stream stream);
        DocumentInfo CreateDocument(CreateDocumentInput input, string url);
        void CreateHistory(CreateHistoryInput input);
        void StoreDocumentFile(DocumentInfo documentInfo, Stream stream);
        void StoreDocumentFile(DocumentInfo documentInfo, string url);
    }
}