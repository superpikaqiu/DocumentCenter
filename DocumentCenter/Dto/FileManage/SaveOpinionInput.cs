using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class SaveOpinionInput
    {
        public int DocumentInfoId { get; set; }
        public string OpinionList { get; set; }
    }
}