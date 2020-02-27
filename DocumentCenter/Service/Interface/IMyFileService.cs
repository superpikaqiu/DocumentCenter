using DocumentCenter.Dto.MyFile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DocumentCenter.Service.Interface
{
    public interface IMyFileService
    {
        GetFileListOutput GetFileList(GetFileListInput input);
        int Upload(UploadInput input);
        bool RemoveFile(string id);
        MyFileDto GetFile(int id);
        void UpdateFileAfterEdit(int id);
    }
}
