using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Domain.CurrentUser
{
    public class CurrentUser
    {
        public static string UserName { get; }
        public static string UserID { get;  }
        private static Dictionary<string,UserModel> UserDic { get; set; }
    }

    class UserModel
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
    }
}