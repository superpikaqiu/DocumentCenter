using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileShare
{
    public class GetShareUserListOutput
    {
        public int Count { get; set; }
        public List<FileUserDto> Items { get; set; }
    }

    
}