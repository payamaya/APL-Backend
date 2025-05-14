using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;


namespace Infrastructure.Repositories
{
    public class UserCourseRepository : BaseRepository<UserCourse>, IUserCourseRepository
    {
        public UserCourseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<UserCourse?> FindAsync(Guid userId, Guid courseId)
        {
            return await _context.UserCourses.FindAsync(userId, courseId);
        }
    }
}
