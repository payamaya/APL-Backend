

namespace Domain.Entities.Base
{
    public class Student: BasePersonEntity
    {
        public User User { get; set; } = null!;
    }
}
