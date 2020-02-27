using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Models
{
    public class FileUser
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public string UserId { get; set; }
        public string Permission { get; set; }
        public DateTime CreationTime { get; set; }
    }
}