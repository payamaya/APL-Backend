using System.ComponentModel.DataAnnotations;
using Domain.Entities;

public class FileRecord
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long Size { get; set; }

    public string MimeType { get; set; } = "application/octet-stream"; // default fallback

    public DateTime UploadedAt { get; set; }

    public Guid? ActivityId { get; set; }
    public Activity Activity { get; set; } // FK link

    //public string MimeType { get; internal set; }
    //public long FileSize { get; internal set; }
    //public DateTime UploadedAt { get; internal set; }
    //public string DownloadUrl { get; internal set; }
}
