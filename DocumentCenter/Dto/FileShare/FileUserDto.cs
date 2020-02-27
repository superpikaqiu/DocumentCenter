using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileShare
{
    public class FileUserDto
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Permission { get; set; }
    }
}