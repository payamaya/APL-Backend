using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IUserService _userService;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository repository, IMapper mapper, AppDbContext dbContext, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto?> GetStudentByIdAsync(Guid id)
        {
            var student = await _repository.GetByIdAsync(id);
            return student == null ? null : _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> CreateStudentAsync(StudentDto dto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Step 1: Create the User
                var userDto = new UserDto
                {
                    Email = dto.Email,
                    Password = "string",
                    Role = Role.Student
                };

                var createdUser = await _userService.CreateUserAsync(userDto);

                // Step 2: Create the Teacher
                var student = _mapper.Map<Student>(dto);
                student.UserId = createdUser.Id;
                student.Email = createdUser.Email;

                await _repository.AddAsync(student);
                await _repository.SaveChangesAsync();

                await transaction.CommitAsync();

                return _mapper.Map<StudentDto>(student);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; // Let the exception propagate
            }
        }

        public async Task<StudentDto> UpdateStudentAsync(StudentDto dto)
        {
            var student = await _repository.GetByIdAsync(dto.UserId);
            if (student == null) throw new Exception("Student not found");

            _mapper.Map(dto, student);
            await _repository.UpdateAsync(student);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null) return false;

            await _repository.DeleteAsync(student);
            return true;
        }
    }
}
