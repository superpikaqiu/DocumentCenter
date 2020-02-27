using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocumentCenter.Models
{
    public class DocumentInfo
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long Size { get; set; }
        public int CurrentVersion { get; set; }
        public string Key { get; set; }
        public string Path { get; set; }
        public string RelativePath { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUserID { get; set; }
        public string CreateUserName { get; set; }
        public DateTime ModifyTime { get; set; }
        public string ModifyUserId { get; set; }
        public string ModifyUserName { get; set; }
        public bool ExternalFlag { get; set; }
        public string ExternalID { get; set; }
        public string ExternalCallbackUrl { get; set; }
        public string ExternalStartEditUrl { get; set; }
        public string ExternalEndEditUrl { get; set; }
        public string ExternalFilePath { get; set; }
        public string ExternalSaveOpinionUrl { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public bool IsEditing { get; set; }
        public string ExtendData { get; set; }
        public string SystemType { get; set; }
    }
}