using System.ComponentModel.DataAnnotations;

namespace filemanagementapi.Domain.UserModel;
public class UserModel
{
    [Key]
    public int Id { get; set; }
    public string? UserName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }

    public bool? admin {get;set;}=false;
}
