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

            CreateMap<TeacherDto, Teacher>();
            CreateMap<Teacher, TeacherDto>();

            CreateMap<StudentDto, Student>();
            CreateMap<Student, StudentDto>();

            CreateMap<ModuleDto, Module>();
            CreateMap<Module, ModuleDto>();

            CreateMap<ActivityDto, Activity>();
            CreateMap<Activity, ActivityDto>();

            CreateMap<CreateUserDto, User>();
            CreateMap<User, UserDto>();

            CreateMap<FileRecord, FileDto>();
            CreateMap<FileDto, FileRecord>();

        }
    }
}