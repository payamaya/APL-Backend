using Domain.Interfaces;
using Infrastructure.Repositories.Interfaces;

public class RepositoryWrapper : IRepositoryWrapper
{
    public IUserRepository Users { get; }
    public IStudentRepository Students { get; }
    public ITeacherRepository Teachers { get; }

    public RepositoryWrapper(
        IUserRepository users,
        IStudentRepository students,
        ITeacherRepository teachers)
    {
        Users = users;
        Students = students;
        Teachers = teachers;
    }
}
