using Domain.Entities;

public class FileRecord
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long Size { get; set; }

    public int ActivityId { get; set; }
    public Activity Activity { get; set; } // FK link
}
