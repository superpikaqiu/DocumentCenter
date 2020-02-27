using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DocumentCenter.Domain.Helper
{
    public class NPOIHelper
    {
        public static byte[] CreateExcelFile()
        {
            XSSFWorkbook book = new XSSFWorkbook();
            ISheet sheet1 = book.CreateSheet("Sheet1");
            MemoryStream memoryStream = new MemoryStream();
            book.Write(memoryStream);
            return memoryStream.ToArray();
        }
    }
}