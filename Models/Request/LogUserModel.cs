using System.ComponentModel.DataAnnotations;


namespace filemanagementapi.Models.Request
{
    public class LogUserModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password {get;set;}
    }
}