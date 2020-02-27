using DocumentCenter.DB;
using DocumentCenter.Dto.FileShare;
using DocumentCenter.Dto.MyFile;
using DocumentCenter.Models;
using DocumentCenter.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentCenter.Service.Implement
{
    public class FileShareService : IFileShareService
    {
        private readonly DocumentDbContext _context = new DocumentDbContext();

        public bool AddShareUser(AddShareUserInput input)
        {
            var fileUsersToRemove = _context.FileUsers.Where(a => a.FileId == input.FileId);
            _context.FileUsers.RemoveRange(fileUsersToRemove);

            foreach(var item in input.FileUserDtos)
            {
                var user = _context.Users.FirstOrDefault(a => a.UserID == item.UserId);
                if (user == null)
                {
                    user = new User
                    {
                        UserID = item.UserId,
                        UserName = item.UserName,
                        CreationTime = DateTime.Now
                    };

                    _context.Users.Add(user);
                }

                var fileUser = new FileUser
                {
                    FileId = input.FileId,
                    UserId = item.UserId,
                    Permission = item.Permission,
                    CreationTime = DateTime.Now
                };

                _context.FileUsers.Add(fileUser);
            }

            _context.SaveChanges();
            
            return true;
        }

        public bool DeleteShareUser(FileUserDto input)
        {
            var fileUser = _context.FileUsers.FirstOrDefault(f => f.Id == input.Id);

            if (fileUser != null)
            {
                _context.FileUsers.Remove(fileUser);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public GetShareFileListOutput GetShareFileList(GetShareFileListInput input)
        {
            var query = from shareuser in _context.FileUsers
                        join personalFile in _context.PersonalFiles on shareuser.FileId equals personalFile.Id
                        join document in _context.DocumentInfos on personalFile.DocumentID equals document.Id
                        where shareuser.UserId == input.UserID
                        select new FileListDto
                        {
                            Id = personalFile.Id,
                            DocumentID = personalFile.DocumentID,
                            Size = document.Size,
                            FileName = document.FileName,
                            Permission = shareuser.Permission,
                            CreateUserName = personalFile.CreateUserName,
                            ModifyUserName = personalFile.ModifyUserName,
                            TempTime = personalFile.ModifyTime,
                            Path = document.Path,
                            RelativePath = document.RelativePath
                        };

            if (!string.IsNullOrEmpty(input.FileName))
            {
                query = query.Where(a => a.FileName.Contains(input.FileName));
            }

            var count = query.Count();
            var tmpList = query.OrderBy(a => a.Id).Skip(input.Limit * (input.Page - 1)).Take(input.Limit).ToList();

            foreach (var item in tmpList)
            {
                item.ModifyTime = item.TempTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return new GetShareFileListOutput
            {
                Count = count,
                Items = tmpList
            };
        }

        public GetShareUserListOutput GetShareUserList(GetShareUserListInput input)
        {
            var query = from fileuser in _context.FileUsers
                        join user in _context.Users
                        on fileuser.UserId equals user.UserID
                        where fileuser.FileId == input.FileId
                        select new FileUserDto
                        {
                            Id = fileuser.Id,
                            UserName = user.UserName,
                            UserId = user.UserID,
                            Permission = fileuser.Permission
                        };
            
            var count = query.Count();

            var tmpList = query.OrderBy(a => a.Id).Skip(input.Limit * (input.Page - 1)).Take(input.Limit).ToList();

            return new GetShareUserListOutput
            {
                Count = count,
                Items = tmpList
            };
        }
    }
}