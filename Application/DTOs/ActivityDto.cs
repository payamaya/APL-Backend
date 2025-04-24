using System;
using System.Collections.Generic;
using Domain.Enums;
using Microsoft.AspNetCore.Http;    // For IFormFile

namespace Application.DTOs
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty; // ⛔ Remove `internal set`
        public string Description { get; set; } = string.Empty; // ⛔ Remove `internal set`

        public string Content { get; set; } = string.Empty;
        public ActivityType ActivityType { get; set; } // ⛔ Remove `internal set`
        public Guid ModuleId { get; set; } // Required foreign key

        public DateTime StartDate { get; set; }

        // Optional depending on type
        public DateTime? EndDate { get; set; } // For assignments
        public List<string>? Questions { get; set; } // For quizzes or polls
        public List<string>? Options { get; set; } // For polls
        public List<CommentDto>? Comments { get; set; } // For discussions
        public List<IFormFile>? Files { get; set; } // For incoming uploads
        public List<string>? AttachmentUrls { get; set; } // For outgoing URLs
    }

}
