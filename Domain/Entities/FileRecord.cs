
public class FileRecord
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long Size { get; set; }

    public string MimeType { get; set; } = "application/octet-stream"; // default fallback

    public DateTime UploadedAt { get; set; }

    public Guid? ActivityId { get; set; }

}
