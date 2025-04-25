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
            CreateMap<CourseDto, Course>();
            CreateMap<Course, CourseDto>();

            CreateMap<ModuleDto, Module>();
            CreateMap<Module, ModuleDto>();

            // ──────────────────────────────────────────────────
            // DTO → Entity: for both Create and Update
            // ──────────────────────────────────────────────────
            //CreateMap<ActivityDto, Activity>()
            //    // on CREATE: ignore Id so the default Guid.NewGuid() is used
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())                           // Ignore incoming Id
            //    .ForMember(dest => dest.Attachments, opt => opt.Ignore());            // Ignore incoming Attachments                                                                                           // we manage URLs in the service, not via DTO

            //// ──────────────────────────────────────────────────
            //// Entity → DTO: for GETs
            //// ──────────────────────────────────────────────────
            //CreateMap<Activity, ActivityDto>()
            //    .ForMember(dest => dest.Files, opt => opt.Ignore())            // incoming-only
            //    .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src =>      // **NEW**
            //        src.Attachments
            //           .Select(a => new AttachmentDto
            //           {
            //               Id = a.Id,
            //               FileName = a.FileName,
            //               Url = $"/api/course/module/{src.ModuleId}/activity/{src.Id}/attachments/{a.Id}"
            //           })
            //           .ToList()
            //    ));
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

            CreateMap<Domain.Entities.Comment, CommentDto>();
            CreateMap<CommentDto, Domain.Entities.Comment>();
        }
    }
}
