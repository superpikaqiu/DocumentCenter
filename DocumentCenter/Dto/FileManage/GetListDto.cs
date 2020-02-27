using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.FileManage
{
    public class GetListDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public DateTime TempTime { get; set; }
        public string CreateTime { get; set; }
        public int DocumentID { get; set; }
    }
}