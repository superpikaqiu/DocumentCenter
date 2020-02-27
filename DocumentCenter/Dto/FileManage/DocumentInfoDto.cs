using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class DocumentInfoDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int CurrentVersion { get; set; }
        public string Key { get; set; }
        public string Path { get; set; }
        public string RelativePath { get; set; }
        public string CreateTime { get; set; }
        public string CreateUserID { get; set; }
        public string CreateUserName { get; set; }
        public bool ExternalFlag { get; set; }
        public string ExternalID { get; set; }
        public string ExternalCallbackUrl { get; set; }
        public string ExternalStartEditUrl { get; set; }
        public string ExternalEndEditUrl { get; set; }
        public string ExternalUserData { get; set; }
    }
}