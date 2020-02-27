using DocumentCenter.Dto.FileShare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentCenter.Service.Interface
{
    public interface IFileShareService
    {
        GetShareUserListOutput GetShareUserList(GetShareUserListInput input);
        bool AddShareUser(AddShareUserInput input);
        bool DeleteShareUser(FileUserDto input);
        GetShareFileListOutput GetShareFileList(GetShareFileListInput input);
    }
}
