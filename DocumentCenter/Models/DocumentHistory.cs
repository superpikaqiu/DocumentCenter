using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DocumentCenter.Models
{
    public class DocumentHistory
    {
        public int Id { get; set; }
        public int DocumentInfoId { get; set; }
        public int Version { get; set; }
        public int PreviousVersion { get; set; }
        public string ServerVersion { get; set; }
        public string Key { get; set; }
        public string Path { get; set; }
        public string Changes { get; set; }
        public string ChangesUrl { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
        public bool AutoSave { get; set; }
    }
}