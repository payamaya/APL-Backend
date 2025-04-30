using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ICourseRepository _courseRepository; // Add ICourseRepository to handle courses
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, ICourseRepository courseRepository, IMapper mapper)
        {
            _repository = repository;
            _courseRepository = courseRepository;  // Inject ICourseRepository to access courses
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            // Optionally hash password if needed
            // user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _repository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task AssignUserToCourseAsync(AssignUserToCourseDto dto)
        {
            var user = await _repository.GetByIdAsync(dto.UserId);
            if (user == null) throw new Exception("User not found");

            var course = await _courseRepository.GetByIdAsync(dto.CourseId); // Use course repository to fetch the course
            if (course == null) throw new Exception("Course not found");

            user.Courses.Add(course);
            await _repository.UpdateAsync(user);
        }

        public async Task<List<UserDto>> GetAllTeachersAsync()
        {
            var teachers = await _repository.GetTeachersAsync();
            return _mapper.Map<List<UserDto>>(teachers);
        }
    }
}
