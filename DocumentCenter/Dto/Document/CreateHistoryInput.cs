using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.Document
{
    public class CreateHistoryInput
    {
        public int DocumentInfoId { get; set; }
        public int Version { get; set; }
        public int PreviousVersion { get; set; }
        public string ServerVersion { get; set; }
        public string Key { get; set; }
        public string Changes { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool AutoSave { get; set; }
        public DateTime CreateTime { get; set; }
        public string FileType { get; set; }
        public string HistoryUrl { get; set; }
        public string ChangesUrl { get; set; }
    }
}