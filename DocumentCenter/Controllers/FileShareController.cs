using DocumentCenter.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using DocumentCenter.Models;
using DocumentCenter.Dto.FileManage;
using DocumentCenter.Dto.FileShare;
using DocumentCenter.Service.Interface;
using DocumentCenter.Service.Implement;
using DocumentCenter.Domain.AuthConfig;

namespace DocumentCenter.Controllers
{
    [Authentication]
    public class FileShareController : Controller
    {
        private readonly IFileShareService fileShareService = new FileShareService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetShareUserList(GetShareUserListInput input)
        {
            var output = fileShareService.GetShareUserList(input);

            return Json(new { code = "0", msg = "", count = output.Count, data = output.Items }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddShareUser(AddShareUserInput input)
        {
            if (fileShareService.AddShareUser(input))
            {
                return Json(new { code = "0" });
            }

            return Json(new { code = "1", msg = "用户已存在" });
        }

        public ActionResult DeleteShareUser(FileUserDto input)
        {
            if (fileShareService.DeleteShareUser(input))
            {
                return Json(new { msg = "success" });
            }
            
            return Json(new { msg = "error" });
        }

        public ActionResult GetShareFileList(GetShareFileListInput input)
        {
            input.UserID = Session["UserID"].ToString();
            var output = fileShareService.GetShareFileList(input);

            return Json(new { code = "0", msg = "", count = output.Count, data = output.Items }, JsonRequestBehavior.AllowGet);
        }

    }
}