using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Base;

namespace Application.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CourseDto, Course>();
            CreateMap<Course, CourseDto>();

            CreateMap<TeacherDto, Teacher>();
            CreateMap<Teacher, TeacherDto>();

            CreateMap<StudentDto, Student>();
            CreateMap<Student, StudentDto>();

            CreateMap<ModuleDto, Module>();
            CreateMap<Module, ModuleDto>();

            CreateMap<ActivityDto, Activity>();
            CreateMap<Activity, ActivityDto>();

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<FileRecord, FileDto>();
            CreateMap<FileDto, FileRecord>();

        }
    }
}