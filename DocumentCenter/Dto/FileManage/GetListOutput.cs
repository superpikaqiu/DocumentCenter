using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class GetListOutput
    {
        public int Count { get; set; }
        public List<GetListDto> Items { get; set; }
    }
}