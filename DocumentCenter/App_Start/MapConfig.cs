using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using DocumentCenter.Controllers.FileManage.Dto;
using DocumentCenter.Dto.Document;
using DocumentCenter.Dto.FileManage;
using DocumentCenter.Dto.FileShare;
using DocumentCenter.Dto.MyFile;
using DocumentCenter.Dto.User;
using DocumentCenter.Models;

namespace DocumentCenter.App_Start
{
    public class MapConfig
    {
        public static void Config()
        {
            Mapper.Initialize(config =>
                {
                    config.CreateMap<DateTime, string>().ConvertUsing(new StringToDateTimeTypeConverter());
                    config.CreateMap<DocumentInfo, DocumentInfoDto>();
                    config.CreateMap<DocumentHistory, DocumentHistoryDto>()
                    .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    config.CreateMap<User, UserDto>();
                    config.CreateMap<FileUser, FileUserDto>();
                    config.CreateMap<UserDto, User>();
                    
                    config.CreateMap<CreateHistoryInput, DocumentHistory>();
                    config.CreateMap<CreateDocumentInput, DocumentInfo>();
                    config.CreateMap<ExternalSaveFileInput, CreateDocumentInput>();
                    config.CreateMap<PersonalFile, MyFileDto>();
                });
        }
    }
    public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            return System.Convert.ToDateTime(source);
        }
    }

    public class StringToDateTimeTypeConverter : ITypeConverter<DateTime, string>
    {

        public string Convert(DateTime source, string destination, ResolutionContext context)
        {
            return source.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}