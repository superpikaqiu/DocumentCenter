using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class OpinionDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public List<OpinionReplyDto> Replys { get; set; }
    }

    public class OpinionReplyDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
    }
}