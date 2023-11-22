using System.ComponentModel.DataAnnotations;

namespace filemanagementapi.Domain.FileModel;
public class FileModel
{
    [Key]
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public DateTime dateTime {get;set;}  
}
