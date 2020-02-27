using AutoMapper;
using Newtonsoft.Json;
using DocumentCenter.DB;
using DocumentCenter.Domain.Helper;
using DocumentCenter.Dto.Common;
using DocumentCenter.Dto.FileManage;
using DocumentCenter.Dto.User;
using DocumentCenter.Models;
using DocumentCenter.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly DocumentDbContext _context = new DocumentDbContext();
        private readonly string CdpmOrigin = System.Configuration.ConfigurationManager.AppSettings["CdpmHost"];

        private readonly string LOGIN_API_CDPM = "/liteproject/webservice/userWebService.asmx/UserLogin";
        public UserDto GetUser(string id)
        {
            var user = _context.Users.FirstOrDefault(a => a.UserID == id);
            return Mapper.Map<UserDto>(user);
        }

        public LoginResultDto Login(LoginDto loginDto)
        {
            var url = CdpmOrigin;
            return LoginToCdpm(loginDto.UserName, loginDto.Password,url);
        }

        public LoginResultDto LoginToCdpm(string workNo,string password,string url)
        {
            url += LOGIN_API_CDPM;

            var param = new
            {
                WorkNo = workNo,
                Password = password
            };

            try
            {
                var result = WebRequestHelper.PostData(url, param);
                var webServiceResult = JsonConvert.DeserializeObject<WebServiceResultDto>(result);
                var data = JsonConvert.DeserializeObject<LoginDataDto>(webServiceResult.D);

                if (data.Result && data.Data.Count > 0)
                {
                    var userData = data.Data[0];
                    HttpContext.Current.Session["UserID"] = userData.ID;
                    HttpContext.Current.Session["UserName"] = userData.Name;

                    SaveUser(new UserDto
                    {
                        UserID = userData.ID,
                        UserName = userData.Name
                    });
                }

                return new LoginResultDto
                {
                    Result = data.Result,
                    Message = data.Message
                };
            }
            catch (Exception e)
            {
                return new LoginResultDto
                {
                    Result = false,
                    Message = "登录失败"
                };
            }

        }

        public bool SaveUser(UserDto userDto)
        {
            if(string.IsNullOrEmpty(userDto.UserID) || string.IsNullOrEmpty(userDto.UserName))
            {
                return true;
            }

            var user = _context.Users.FirstOrDefault(a => a.UserID == userDto.UserID);

            if(user == null)
            {
                user = Mapper.Map<User>(userDto);
                user.CreationTime = DateTime.Now;
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            return true;
        }
    }
}