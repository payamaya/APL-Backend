using Infrastructure.Repositories.Interfaces;

namespace Domain.Interfaces
{
    public interface IRepositoryWrapper
    {
        IUserRepository Users { get; }
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }
    }

}
