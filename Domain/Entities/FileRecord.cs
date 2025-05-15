
public class FileRecord
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public long Size { get; set; }

    public string MimeType { get; set; } = "application/octet-stream"; // default fallback

    public DateTime UploadedAt { get; set; }

    public Guid? ActivityId { get; set; }

}
