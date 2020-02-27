using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.MyFile
{
    public class GetFileListOutput
    {
        public int Count { get; set; }
        public List<FileListDto> Items { get; set; }
    }
}