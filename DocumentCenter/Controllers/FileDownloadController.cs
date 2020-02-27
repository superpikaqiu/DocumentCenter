using DocumentCenter.DB;
using DocumentCenter.Domain.Helper;
using DocumentCenter.Dto.FileDownload;
using DocumentCenter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentCenter.Controllers.FileStore
{
    public class FileDownloadController : Controller
    {
        // GET: FileDownload
        public ActionResult Index()
        {
            return View();
        }

        public FileResult DownloadTempFile(string filePath)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);
                var bytes = System.IO.File.ReadAllBytes(filePath);
                System.IO.File.Delete(filePath);
                return File(bytes, "application/octet-stream ; Charset=UTF8", fileName);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult GetFileUrl(List<GetFileUrlDto> items)
        {
            var filePath = ZipHelper.CreateZip(items);
            return Json(new { filePath });
        }

        public FileResult DownloadFromPath(string path)
        {
            string fileName = path.Substring(path.LastIndexOf("\\") + 1);
            return File(path, "application/octet-stream ; Charset=UTF8", fileName);
        }

        public FileResult Download(string path,string fileName)
        {
            return File(path, "application/octet-stream ; Charset=UTF8", fileName);
        }

        public FileResult DownloadFromRelativePath(string path)
        {
            string absolutePath = Server.MapPath(path);
            string fileName = absolutePath.Substring(absolutePath.LastIndexOf("\\") + 1);
            return File(absolutePath, "application/octet-stream ; Charset=UTF8", fileName);
        }

    }
}