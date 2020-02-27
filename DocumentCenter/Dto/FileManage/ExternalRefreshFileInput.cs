using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Controllers.FileManage.Dto
{
    public class ExternalRefreshFileInput
    {
        public string ID { get; set; }
        public string NewID { get; set; }
        public string FilePath { get; set; }
    }
}