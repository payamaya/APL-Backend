using System.Linq;                       // For LINQ Select
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<TeacherDto, Teacher>();
            CreateMap<Teacher, TeacherDto>();

            CreateMap<CourseDto, Course>();
            CreateMap<Course, CourseDto>();

            CreateMap<ModuleDto, Module>();
            CreateMap<Module, ModuleDto>();

            CreateMap<ActivityDto, Activity>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.AttachmentUrls, o => o.Ignore());

            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.Files, o => o.Ignore())
                .ForMember(d => d.AttachmentUrls, o => o.MapFrom(src => src.AttachmentUrls));

            // Attachment DTO ↔ Entity (if you ever need reverse mapping)
            // ──────────────────────────────────────────────────
            CreateMap<AttachmentDto, ActivityAttachment>()                                    // **NEW**
                .ForMember(dest => dest.Data, opt => opt.Ignore())                       // **NEW**
                .ForMember(dest => dest.Activity, opt => opt.Ignore())                       // **NEW** avoid circular
                .ForMember(dest => dest.ActivityId, opt => opt.Ignore());                     // **NEW** set in service

        }
    }
}