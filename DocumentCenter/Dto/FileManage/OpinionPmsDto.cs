using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class OpinionPmsDto
    {
        public string result_id { get; set; }
        public string type { get; set; }
        public List<OpinionItemPmsDto> opinion_list { get; set; }
    }

    public class OpinionItemPmsDto
    {
        public string opinion_content { get; set; }
        public string create_uid { get; set; }
        public List<OpinionReplyPmsDto> reply_opinion_list { get; set; }
    }

    public class OpinionReplyPmsDto
    {
        public string content { get; set; }
        public string create_uid { get; set; }
    }
}