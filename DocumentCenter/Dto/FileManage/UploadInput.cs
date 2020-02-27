using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class UploadInput
    {
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Path { get; set; }
        public string HistoryPath { get; set; }
    }
}