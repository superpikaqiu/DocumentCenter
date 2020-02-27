using DocumentCenter.Dto.FileManage;
using DocumentCenter.Dto.MyFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileShare
{
    public class GetShareFileListOutput
    {
        public int Count { get; set; }
        public List<FileListDto> Items { get; set; }
    }
}