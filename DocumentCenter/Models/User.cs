using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DocumentCenter.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}