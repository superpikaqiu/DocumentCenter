using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Controllers.FileManage.Dto
{
    public class ExternalSaveFileInput
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string ExternalFilePath { get; set; }
        public string ExternalCallbackUrl { get; set; }
        public string ExternalStartEditUrl { get; set; }
        public string ExternalSaveOpinionUrl { get; set; }
        public string ExternalID { get; set; }
        public string CreateUserID { get; set; }
        public string CreateUserName { get; set; }
        public string ExtendData { get; set; }
        public string SystemType { get; set; }
    }
}