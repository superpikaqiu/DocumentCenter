using DocumentCenter.Dto.FileManage;
using DocumentCenter.DB;
using DocumentCenter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using System.Collections;
using DocumentCenter.Controllers.FileManage.Dto;
using Newtonsoft.Json;
using DocumentCenter.Domain.Helper;
using DocumentCenter.Service.Interface;
using DocumentCenter.Service.Implement;

namespace DocumentCenter.Controllers
{
    public class FileManageController : Controller
    {
        private readonly IFileManageService fileManageService = new FileManageService();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyFile()
        {
            return View();
        }

        public ActionResult Share()
        {
            return View();
        }

        public ActionResult ExternalEditFile()
        {
            return View();
        }

        public ActionResult ExternalViewFile()
        {
            return View();
        }

        public ActionResult Editor()
        {
            return View();
        }

        public ActionResult ExternalEditPms()
        {
            return View();
        }

        public ActionResult GetFileInfo(int id)
        {
            try
            {
                return Json(new { DocumentInfo = fileManageService.GetFileInfo(id) }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return new EmptyResult();
            }
        }

        public ActionResult GetFileKey(int id)
        {
            try
            {

                return Json(new { Key = fileManageService.GetFileKey(id) });
            }
            catch
            {
                return new EmptyResult();
            }

        }

        /// <summary>
        /// onlyoffice保存文件回调
        /// 状态：
        /// 2：更改文件后关闭页面
        /// 4：未更改文件关闭页面
        /// 6：手动保存
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveFile()
        {
            string body;
            using (var reader = new StreamReader(Request.InputStream))
                body = reader.ReadToEnd();

            SaveFileInput input = new SaveFileInput
            {
                Data = body,
                ServerOrigin = "http://" + Request.ServerVariables.Get("HTTP_HOST").ToString()
            };

            if (fileManageService.SaveFile(input))
            {
                return Json(new { error = "0" });
            }

            return Json(new { error = "1" });
        }

        public ActionResult GetFileHistory(int id)
        {
            var output = fileManageService.GetFileHistory(id);
            return Json(new { CurrentVersion = output.CurVersion, History = output.Items }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExternalSaveFileByYF(ExternalSaveFileInput input)
        {
            var id = fileManageService.ExternalSaveFileByYF(input);

            return Json(new { Id = id });
        }


        /// <summary>
        /// 外部系统修改文件后，同步回文档中心
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ActionResult ExternalRefreshFile(ExternalRefreshFileInput input)
        {
            fileManageService.ExternalRefreshFile(input);

            return Json(new { Code = "OK" });
        }

        public ActionResult UploadDesignVolumeProduct(UploadDesignVolumeProductInput input)
        {
            var uploadFile = Request.Files[0];
            input.FileName = uploadFile.FileName;
            input.FileType = input.FileName.Substring(input.FileName.LastIndexOf('.') + 1);
            var id = fileManageService.UploadDesignVolumeProduct(input, uploadFile.InputStream);

            return Json(new { msg = "success", documentId = id });
        }

        public ActionResult GetOnlyOfficePath()
        {
            var onlyofficePath = System.Configuration.ConfigurationManager.AppSettings["OnlyOffice.DocumentServer"];
            return Json(new { Path = onlyofficePath }, JsonRequestBehavior.AllowGet);
        }

        public void UpdateEditingUrl(UpdateEditingUrlInput input)
        {
            fileManageService.UpdateEditingUrl(input);
        }

        public JsonResult GetFileSyncList()
        {
            var result = fileManageService.GetSyncDocumentList();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public void FileSyncCallback(List<FileSyncResultDto> fileSyncResultDtos)
        {
            fileManageService.FileSyncCallback(fileSyncResultDtos);
        }

        public JsonResult ExternalSaveFileByPms(ExternalSaveFileInput input)
        {
            var result = fileManageService.ExternalSaveFileByPms(input);

            return Json(result);
        }

        public void SaveOpinionList(SaveOpinionInput input)
        {
            fileManageService.SaveOpinionList(input);
        }
    }
}