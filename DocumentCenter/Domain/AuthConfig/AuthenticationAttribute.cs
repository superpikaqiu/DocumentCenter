using Newtonsoft.Json;
using DocumentCenter.Domain.Helper;
using DocumentCenter.Dto.User;
using DocumentCenter.Service.Implement;
using DocumentCenter.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DocumentCenter.Domain.AuthConfig
{
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        private readonly IUserService userService = new UserService();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            try
            {
                var userInfo = filterContext.HttpContext.Request.QueryString["userInfo"];

                //快速登录
                if (userInfo != null)
                {
                    userInfo = userInfo.Replace(" ", "+");
                    var jsonStr = DESHelper.DesDecrypt(userInfo);
                    var userInfoDto = JsonConvert.DeserializeObject<QuickLoginDto>(jsonStr);
                    var url = filterContext.HttpContext.Request.QueryString["cdpmOrigin"];
                    var result = userService.LoginToCdpm(userInfoDto.UserName, userInfoDto.UserPwd, url);

                    if (result.Result)
                    {
                        filterContext.HttpContext.Session["CdpmOrigin"] = filterContext.HttpContext.Request.QueryString["cdpmOrigin"];
                        filterContext.HttpContext.Session["DesignVolumeQuery"] = filterContext.HttpContext.Request.Url.Query;
                        filterContext.HttpContext.Session["DesignVolumeID"] = filterContext.HttpContext.Request.QueryString["designVolumeID"];
                        filterContext.HttpContext.Session["LoginFromVTask"] = true;
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("/login/index");
                    }
                }
                else
                {
                    if (filterContext.HttpContext.Session["UserID"] == null)
                    {
                        filterContext.Result = new RedirectResult("/login/index");
                    }
                }
            }
            catch
            {
                filterContext.Result = new RedirectResult("/login/index");
            }

            base.OnActionExecuting(filterContext);
        }


    }
}