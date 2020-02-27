using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.Document
{
    public class CreateDocumentInput
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string CreateUserID { get; set; }
        public string CreateUserName { get; set; }
        public bool ExternalFlag { get; set; }
        public string ExternalID { get; set; }
        public string ExternalCallbackUrl { get; set; }
        public string ExternalStartEditUrl { get; set; }
        public string ExternalSaveOpinionUrl { get; set; }
        public string ExternalUserId { get; set; }
        public string ExternalUserName { get; set; }
        public string ExternalFilePath { get; set; }
        public string ExtendData { get; set; }
        public string SystemType { get; set; }
    }
}