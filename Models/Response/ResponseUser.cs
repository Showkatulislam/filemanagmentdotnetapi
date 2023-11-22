using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace filemanagementapi.Models.Response
{
    public class ResponseUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }

    }
}