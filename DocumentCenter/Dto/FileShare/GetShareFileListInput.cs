using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileShare
{
    public class GetShareFileListInput
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public string UserID { get; set; }
        public string FileName { get; set; }
    }
}