using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserCourseRepository
    {
        Task<UserCourse?> FindAsync(Guid userId, Guid courseId);
        Task AddAsync(UserCourse uc);
        Task RemoveAsync(UserCourse uc);
        Task SaveChangesAsync();
    }
}
