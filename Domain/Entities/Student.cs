

using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Student: BasePersonEntity
    {
        public User User { get; set; } = null!;
    }
}
