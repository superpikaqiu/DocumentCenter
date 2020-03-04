using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Dto.User
{
    
    public class LoginDataDto
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public List<UserData> Data { get; set; }
        public int TotalCount { get; set; }
    }

    public class UserData
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string YFMIS3 { get; set; }
    }
}