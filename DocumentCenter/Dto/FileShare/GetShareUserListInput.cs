using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileShare
{
    public class GetShareUserListInput
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int FileId { get; set; }
    }
}