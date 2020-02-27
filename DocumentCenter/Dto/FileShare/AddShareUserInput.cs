using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileShare
{
    public class AddShareUserInput
    {
        public int FileId { get; set; }
        public List<FileUserDto> FileUserDtos { get; set; }
    }

    
}