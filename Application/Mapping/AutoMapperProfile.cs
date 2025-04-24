using System.Xml.Linq;
using Application.DTOs;
using AutoMapper;
using DocumentFormat.OpenXml.Presentation;
using Domain.Entities;


namespace Application.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CourseDto, Course>();
            CreateMap<Course, CourseDto>();

            CreateMap<ModuleDto, Module>();
            CreateMap<Module, ModuleDto>();

            CreateMap<ActivityDto, Activity>();
            CreateMap<Activity, ActivityDto>();
        }
    }
}
