using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class GetFileInfoListInput
    {
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}