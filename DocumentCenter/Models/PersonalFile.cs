using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Models
{
    public class PersonalFile
    {
        public int Id { get; set; }
        public int DocumentID { get; set; }
        public string CreateType { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string CreateUserID { get; set; }
        public string CreateUserName { get; set; }
        public DateTime CreateTime { get; set; }
        public string ModifyUserID { get; set; }
        public string ModifyUserName { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}