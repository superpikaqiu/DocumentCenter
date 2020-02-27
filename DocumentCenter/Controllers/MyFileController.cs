using DocumentCenter.Domain.AuthConfig;
using DocumentCenter.Dto.MyFile;
using DocumentCenter.Service.Implement;
using DocumentCenter.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentCenter.Controllers
{
    [Authentication]
    public class MyFileController : Controller
    {
        private readonly IMyFileService myFileService = new MyFileService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetFileList(GetFileListInput input)
        {
            input.UserID = Session["UserID"].ToString();

            var output = myFileService.GetFileList(input);
            return Json(new { code = "0", msg = "", count = output.Count, data = output.Items }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Upload(UploadInput input)
        {
            myFileService.Upload(input);

            return Json(new { msg = "success" });
        }

        public JsonResult RemoveFile(string id)
        {
            if (myFileService.RemoveFile(id))
            {
                return Json(new { msg = "success", code = "0" });
            }

            return Json(new { msg = "error", code = "1" });
        }

        public JsonResult GetFile(int id)
        {
            return Json(myFileService.GetFile(id));
        }

        public void UpdateFileAfterEdit(int id)
        {
            myFileService.UpdateFileAfterEdit(id);
        }
    }
}