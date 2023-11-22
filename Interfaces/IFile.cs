using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace filemanagementapi.Interfaces
{
    public interface IFile
    {
        Tuple<int,string> SaveFile(IFormFile formFile);
        bool DeleteFile(string file);
    }
}