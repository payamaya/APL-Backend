using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Repositories.Interfaces;
using Application.Interfaces;
using Application.DTOs.Base;

namespace Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repository;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CourseDto> CreateCourseAsync(CourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            await _repository.AddAsync(course);
            return _mapper.Map<CourseDto>(course);
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null) return false;

            await _repository.DeleteAsync(course);
            return true;
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }

        public async Task<CourseDto?> GetCourseByIdAsync(Guid id)
        {
            var course = await _repository.GetByIdAsync(id);
            return course == null ? null : _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> UpdateCourseAsync(CourseDto dto)
        {
            var course = await _repository.GetByIdAsync(dto.Id);
            if (course == null) throw new Exception("Course not found");

            _mapper.Map(dto, course);
            await _repository.UpdateAsync(course);
            return _mapper.Map<CourseDto>(course);
        }
    }
}

//using Application.DTOs;
//using Application.Interfaces;
//using Application.Services.Base;
//using AutoMapper;
//using Domain.Entities;
//using Infrastructure.Repositories.Interfaces;

//namespace Application.Services
//{
//    public class CourseService : CrudService<Course, CourseDto>, ICourseService
//    {
//        public CourseService(ICourseRepository repository, IMapper mapper)
//            : base(repository, mapper)
//        {
//        }
//    }
//}
