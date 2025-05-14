using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserCourseRepository : BaseRepository<UserCourse>, IUserCourseRepository
    {
        public UserCourseRepository(AppDbContext context) : base(context)
        {
            _ctx.UserCourses.Add(uc);
            return _ctx.SaveChangesAsync();
        }

        public async Task<UserCourse?> FindAsync(Guid userId, Guid courseId)
        {
            return await _context.UserCourses.FindAsync(userId, courseId);
        }

        public Task SaveChangesAsync() => _ctx.SaveChangesAsync();
    }
}
