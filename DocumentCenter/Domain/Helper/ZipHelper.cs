using ICSharpCode.SharpZipLib.Zip;
using DocumentCenter.Dto.FileDownload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DocumentCenter.Domain.Helper
{
    public class ZipHelper
    {
        private static readonly string FilePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"];
        /// <summary>
        /// 压缩多个现有文件
        /// </summary>
        /// <param name="zipName">压缩包名称</param>
        /// <param name="fileList">现在有文件路径</param>
        public static string CreateZip(List<GetFileUrlDto> items)
        {
            if(items.Count == 0)
            {
                return null;
            }

            var tmpPath = HttpContext.Current.Server.MapPath(FilePath);
            var zipName = $"{items[0].FileName}等{items.Count}个文件.zip";
            var storePath = Path.Combine(tmpPath, zipName);
            
            using (var zip = ZipFile.Create(storePath))
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(tmpPath);
                zip.BeginUpdate();
                foreach (var item in items)
                {
                    zip.Add(Path.GetFileName(item.FilePath),item.FileName);
                }
                zip.CommitUpdate();
                Directory.SetCurrentDirectory(currentDirectory);
            }

            return storePath;
            
        }
    }
}