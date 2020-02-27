using DocumentCenter.Dto.FileManage;
using DocumentCenter.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentCenter.Service.Interface
{
    public interface IUserService
    {
        UserDto GetUser(string id);
        bool SaveUser(UserDto userDto);
        LoginResultDto Login(LoginDto loginDto);
        LoginResultDto LoginToCdpm(string workNo, string password, string url);
    }
}
