using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class SyncDocumentDto
    {
        public string originalFileUrl { get; set; }
        public string modifyFileUrl { get; set; }
        public int modifyFileId { get; set; }
    }
}