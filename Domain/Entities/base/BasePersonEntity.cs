
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Base
{
    public abstract class BasePersonEntity: BaseUserEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        [Phone]
        [MaxLength(20)]
        public string Telephone { get; set; } = string.Empty;
    }
}
