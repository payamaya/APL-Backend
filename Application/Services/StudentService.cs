using Application.DTOs;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data;


namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepositoryWrapper _repos;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public StudentService(IRepositoryWrapper repos, AppDbContext dbContext, IMapper mapper)
        {
            _repos = repos;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<StudentDto> CreateStudentAsync(StudentDto dto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Step 1: Create the User
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Telephone = dto.Telephone,
                    Address = dto.Address,
                    Email = dto.Email,
                    Password = PasswordHasher.Hash(dto.Password),
                    Role = Role.Student,
                    EmailConfirmed = false,
                    IsOtpVerified = false, // <- Add this flag on the user entity
                    CreatedAt = DateTime.UtcNow
                };

                await _repos.Users.AddAsync(user);

                // Step 2: Create the Student
                await _repos.Students.AddAsync(new Student
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Telephone = user.Telephone,
                    Address = user.Address,
                    CreatedAt = user.CreatedAt
                });

                await _repos.Students.SaveChangesAsync();

                await transaction.CommitAsync();

                dto.Id = user.Id;
                return _mapper.Map<StudentDto>(dto);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; // Let the exception propagate
            }
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _repos.Students.GetAllAsync();
            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto?> GetStudentByIdAsync(Guid id)
        {
            var student = await _repos.Students.GetByIdAsync(id);
            return student == null ? null : _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> UpdateStudentAsync(StudentDto dto)
        {
            var student = await _repos.Students.GetByIdAsync(dto.Id);
            if (student == null) throw new Exception("Student not found");

            _mapper.Map(dto, student);
            await _repos.Students.UpdateAsync(student);
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            var student = await _repos.Students.GetByIdAsync(id);
            if (student == null) return false;

            await _repos.Students.DeleteAsync(student);
            return true;
        }
    }
}

//using Application.DTOs;
//using Application.Interfaces;
//using Application.Services.Base;
//using AutoMapper;
//using Domain.Enums;
//using Domain.Entities;
//using Infrastructure.Data;
//using Infrastructure.Repositories.Interfaces;

//namespace Application.Services
//{
//    public class StudentService : CrudService<Student, StudentDto>, IStudentService
//    {
//        private readonly AppDbContext _dbContext;
//        private readonly IUserService _userService;
//        public StudentService(AppDbContext dbContext, IUserService userService, IStudentRepository repository, IMapper mapper)
//            : base(repository, mapper)
//        {
//            _dbContext = dbContext;
//            _userService = userService;
//        }

//        public async Task<StudentDto> CreateStudentAsync(StudentDto dto)
//        {
//            using var transaction = await _dbContext.Database.BeginTransactionAsync();

//            try
//            {
//                // Step 1: Create the User
//                var userDto = new UserDto
//                {
//                    Email = dto.Email,
//                    Password = "string", // a temporary password for students to join!
//                    Role = Role.Student
//                };

//                var createdUser = await _userService.CreateUserAsync(userDto);

//                // Step 2: Create the Student
//                var student = _mapper.Map<Student>(dto);
//                student.Id = createdUser.Id;
//                student.Email = createdUser.Email;

//                await _repository.AddAsync(student);
//                await _repository.SaveChangesAsync();

//                await transaction.CommitAsync();

//                return _mapper.Map<StudentDto>(student);
//            }
//            catch
//            {
//                await transaction.RollbackAsync();
//                throw; // Let the exception propagate
//            }
//        }
//    }
//}
