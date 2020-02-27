using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Controllers.FileManage.Dto
{
    public class UploadDesignVolumeProductInput
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}