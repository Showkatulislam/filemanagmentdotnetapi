using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace filemanagementapi.Models.Request;
public class RequestFileModel
{
    public string FileName { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public IFormFile formFile { get; set; }
}
