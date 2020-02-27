using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.MyFile
{
    public class MyFileDto
    {
        public int Id { get; set; }
        public int DocumentID { get; set; }
        public string CreateType { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string UploadFileName { get; set; }
    }
}