using Domain.Entities;


namespace Domain.Interfaces
{
    public interface IUserCourseRepository: IBaseRepository<UserCourse>
    {
        Task<UserCourse?> FindAsync(Guid userId, Guid courseId);

    }
}
