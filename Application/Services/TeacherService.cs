using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Base;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _repository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;

        public TeacherService(ITeacherRepository repository, IMapper mapper, AppDbContext dbContext, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _dbContext = dbContext;
            _userService = userService;
        }

        [HttpPost("create-teacher")]
        public async Task<TeacherDto> CreateTeacherAsync(TeacherDto dto)
        {

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Step 1: Create the User
                var userDto = new UserDto
                {
                    Email = dto.Email,
                    Password = "string",
                    Role = Role.Teacher
                };

                var createdUser = await _userService.CreateUserAsync(userDto);

                // Step 2: Create the Teacher
                var teacher = _mapper.Map<Teacher>(dto);
                teacher.UserId = createdUser.Id;
                teacher.Email = createdUser.Email;

                await _repository.AddAsync(teacher);
                await _repository.SaveChangesAsync();

                await transaction.CommitAsync();

                return _mapper.Map<TeacherDto>(teacher);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; // Let the exception propagate
            }
        }

        public async Task<bool> DeleteTeacherAsync(Guid id)
        {
            var teacher = await _repository.GetByIdAsync(id);
            if (teacher == null) return false;

            await _repository.DeleteAsync(teacher);
            return true;
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            var teachers = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(Guid id)
        {
            var teacher = await _repository.GetByIdAsync(id);
            return teacher == null ? null : _mapper.Map<TeacherDto>(teacher);
        }

        public async Task<TeacherDto> UpdateTeacherAsync(TeacherDto dto)
        {
            var teacher = await _repository.GetByIdAsync(dto.UserId);
            if (teacher == null) throw new Exception("Teacher not found");

            _mapper.Map(dto, teacher);
            await _repository.UpdateAsync(teacher);
            return _mapper.Map<TeacherDto>(teacher);
        }
    }
}
