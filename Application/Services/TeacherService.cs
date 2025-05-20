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
    public class TeacherService : ITeacherService
    {
        private readonly IRepositoryWrapper _repos;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public TeacherService(IRepositoryWrapper repos, AppDbContext dbContext, IMapper mapper)
        {
            _repos = repos;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TeacherDto> CreateTeacherAsync(TeacherDto dto)
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
                await _repos.Teachers.AddAsync(new Teacher
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Telephone = user.Telephone,
                    Address = user.Address,
                    CreatedAt = user.CreatedAt
                });

                await _repos.Teachers.SaveChangesAsync();

                await transaction.CommitAsync();

                dto.Id = user.Id;
                return _mapper.Map<TeacherDto>(dto);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; // Let the exception propagate
            }
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            var teachers = await _repos.Teachers.GetAllAsync();
            return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(Guid id)
        {
            var teacher = await _repos.Teachers.GetByIdAsync(id);
            return teacher == null ? null : _mapper.Map<TeacherDto>(teacher);
        }

        public async Task<TeacherDto> UpdateTeacherAsync(TeacherDto dto)
        {
            var teacher = await _repos.Teachers.GetByIdAsync(dto.Id);
            if (teacher == null) throw new Exception("Teacher not found");

            _mapper.Map(dto, teacher);
            await _repos.Teachers.UpdateAsync(teacher);
            return _mapper.Map<TeacherDto>(teacher);
        }

        public async Task<bool> DeleteTeacherAsync(Guid id)
        {
            var teacher = await _repos.Teachers.GetByIdAsync(id);
            if (teacher == null) return false;

            await _repos.Teachers.DeleteAsync(teacher);
            return true;
        }
    }
}

//using Application.DTOs;
//using Application.Interfaces;
//using Application.Services.Base;
//using AutoMapper;
//using Domain.Entities;
//using Domain.Enums;
//using Infrastructure.Data;
//using Infrastructure.Repositories.Interfaces;

//namespace Application.Services
//{
//    public class TeacherService : CrudService<Teacher, TeacherDto>, ITeacherService
//    {
//        private readonly AppDbContext _dbContext;
//        private readonly IUserService _userService;
//        public TeacherService(AppDbContext dbContext, IUserService userService, ITeacherRepository repository, IMapper mapper)
//            : base(repository, mapper)
//        {
//            _dbContext = dbContext;
//            _userService = userService;
//        }
//        public async Task<TeacherDto> CreateTeacherAsync(TeacherDto dto)
//        {

//            using var transaction = await _dbContext.Database.BeginTransactionAsync();

//            try
//            {
//                // Step 1: Create the User
//                var userDto = new UserDto
//                {
//                    Email = dto.Email,
//                    Password = "string",
//                    Role = Role.Teacher
//                };

//                var createdUser = await _userService.CreateUserAsync(userDto);

//                // Step 2: Create the Teacher
//                var teacher = _mapper.Map<Teacher>(dto);
//                teacher.Id = createdUser.Id;
//                teacher.Email = createdUser.Email;

//                await _repository.AddAsync(teacher);
//                await _repository.SaveChangesAsync();

//                await transaction.CommitAsync();

//                return _mapper.Map<TeacherDto>(teacher);
//            }
//            catch
//            {
//                await transaction.RollbackAsync();
//                throw; // Let the exception propagate
//            }
//        }
//    }
//}
