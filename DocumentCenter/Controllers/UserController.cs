using DocumentCenter.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DocumentCenter.Dto.FileManage;
using DocumentCenter.Domain.CurrentUser;
using DocumentCenter.Service.Interface;
using DocumentCenter.Service.Implement;
using DocumentCenter.Dto.User;

namespace DocumentCenter.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService = new UserService();
        private readonly string CdpmOrigin = System.Configuration.ConfigurationManager.AppSettings["CdpmHost"];

        public ActionResult GetUserInfo()
        {
            var cdpmOrigin = Session["CdpmOrigin"]?.ToString();
            if (string.IsNullOrEmpty(cdpmOrigin))
            {
                cdpmOrigin = CdpmOrigin;
            }
            var vtask = Session["LoginFromVTask"] == null ? false : true;

            return Json(new
            {
                UserId = Session["UserID"].ToString(),
                UserName = Session["UserName"].ToString(),
                CdpmOrigin = cdpmOrigin,
                LoginFromVTask = vtask,
                DesignVolumeID = Session["DesignVolumeID"]
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddUser(UserDto input)
        {
            userService.SaveUser(input);

            return Json(new { Code = "Success" });
        }

        public JsonResult Login(LoginDto loginDto)
        {
            var result = userService.Login(loginDto);
            return Json(result);
        }

        public void Logout()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
        }

        public JsonResult GetCdpmOrigin()
        {
            var cdpmOrigin = Session["CdpmOrigin"]?.ToString();
            if (string.IsNullOrEmpty(cdpmOrigin))
            {
                cdpmOrigin = CdpmOrigin;
            }

            return Json(new
            {
                CdpmOrigin = cdpmOrigin
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserCenter()
        {
            var userCenter = System.Configuration.ConfigurationManager.AppSettings["UserCenter"];
            return Json(new
            {
                UserCenter = userCenter
            }, JsonRequestBehavior.AllowGet);
        }
    }
}