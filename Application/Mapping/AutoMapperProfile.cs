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
        }
    }
}
