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
    public class UserCourseRepository : IUserCourseRepository
    {
        private readonly AppDbContext _ctx;
        public UserCourseRepository(AppDbContext ctx) => _ctx = ctx;

        public Task<UserCourse?> FindAsync(Guid u, Guid c)
            => _ctx.UserCourses.FindAsync(u, c).AsTask();

        public Task AddAsync(UserCourse uc)
        {
            _ctx.UserCourses.Add(uc);
            return _ctx.SaveChangesAsync();
        }

        public Task RemoveAsync(UserCourse uc)
        {
            _ctx.UserCourses.Remove(uc);
            return _ctx.SaveChangesAsync();
        }

        public Task SaveChangesAsync() => _ctx.SaveChangesAsync();
    }
}
