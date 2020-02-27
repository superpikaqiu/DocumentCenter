using DocumentCenter.Controllers.FileManage.Dto;
using DocumentCenter.Dto.FileManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DocumentCenter.Service.Interface
{
    public interface IFileManageService
    {
        DocumentInfoDto GetFileInfo(int id);
        string GetFileKey(int id);
        GetFileHistoryOutput GetFileHistory(int id);
        int ExternalSaveFile(ExternalSaveFileInput input);
        bool SaveFile(SaveFileInput input);
        int UploadDesignVolumeProduct(UploadDesignVolumeProductInput input, Stream stream);
        bool ExternalRefreshFile(ExternalRefreshFileInput input);
        void UpdateEditingUrl(UpdateEditingUrlInput input);
        List<SyncDocumentDto> GetSyncDocumentList();
        void FileSyncCallback(List<FileSyncResultDto> fileSyncResultDtos);
        ExternalSaveFileByPmsOutput ExternalSaveFileByPms(ExternalSaveFileInput input);
        int ExternalSaveFileByYF(ExternalSaveFileInput input);
        void SaveOpinionList(SaveOpinionInput input);
    }
}
