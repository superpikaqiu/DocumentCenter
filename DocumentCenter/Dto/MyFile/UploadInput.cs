using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.MyFile
{
    public class UploadInput
    {
        public int Id { get; set; }
        public string CreateType { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string ShareUserList { get; set; }
    }
}