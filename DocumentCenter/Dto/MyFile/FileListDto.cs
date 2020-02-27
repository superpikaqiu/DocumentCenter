using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.MyFile
{
    public class FileListDto
    {
        public int Id { get; set; }
        public string CreateType { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Permission { get; set; }
        public long Size { get; set; }
        public string CreateUserName { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime TempTime { get; set; }
        public string ModifyTime { get; set; }
        public int DocumentID { get; set; }
        public string Path { get; set; }
        public string RelativePath { get; set; }
    }
}