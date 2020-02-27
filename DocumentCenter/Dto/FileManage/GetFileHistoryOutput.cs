using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class GetFileHistoryOutput
    {
        public int CurVersion { get; set; } 
        public List<DocumentHistoryDto> Items { get; set; }
    }
}