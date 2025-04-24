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
            CreateMap<ActivityDto, Activity>()  
                // on CREATE: ignore Id so the default Guid.NewGuid() is used
                .ForMember(dest => dest.Id, opt => opt.Ignore())                           // ← CHANGED
                                                                                           // we manage URLs in the service, not via DTO
                .ForMember(dest => dest.AttachmentUrls, opt => opt.Ignore());              // ← CHANGED

            // ──────────────────────────────────────────────────
            // Entity → DTO: for GETs
            // ──────────────────────────────────────────────────
            CreateMap<Activity, ActivityDto>()
                // incoming-only
                .ForMember(dest => dest.Files, opt => opt.Ignore())
                // outgoing: include the saved URLs
                .ForMember(dest => dest.AttachmentUrls, opt => opt.MapFrom(src => src.AttachmentUrls));

            CreateMap<Domain.Entities.Comment, CommentDto>();
            CreateMap<CommentDto, Domain.Entities.Comment>();
        }
    }
}
