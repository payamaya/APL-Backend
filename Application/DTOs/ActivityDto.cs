
using Application.DTOs.Base;
using Domain.Enums;

namespace Application.DTOs
{
    public class ActivityDto: BaseTimeDto
    {
        public ActivityType ActivityType { get; set; } // ⛔ Remove `internal set`
        public Guid ModuleId { get; set; } // Required foreign key
        public List<string>? Questions { get; set; } // For quizzes or polls
        public List<string>? Options { get; set; } // For polls
    }

}
