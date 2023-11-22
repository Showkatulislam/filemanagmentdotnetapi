using filemanagementapi.Interfaces;

namespace filemanagementapi.Services
{
    public class FileServices : IFile
    {
        private readonly IWebHostEnvironment _env;
        public FileServices(IWebHostEnvironment env)
        {
            _env = env;
        }
        public Tuple<int, string> SaveFile(IFormFile formFile)
        {
            try
            {
                var rootDir = _env.ContentRootPath;
                var path = Path.Combine(rootDir, "Uploads");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(formFile.FileName);

                string UniquePath = Guid.NewGuid().ToString();

                var FileName = UniquePath + ext;

                var FileStorePath = Path.Combine(path, FileName);

                var stream = new FileStream(FileStorePath, FileMode.Create);


                formFile.CopyToAsync(stream);

                stream.Close();


                return new Tuple<int, string>(1, FileName);
            }
            catch (System.Exception)
            {

                return new Tuple<int, string>(0, "Erro has occured");
            }
        }

        public bool DeleteFile(string file)
        {
            /*             var rootDir=_env.ContentRootPath;
                        var path = Path.Combine(rootDir, "Uploads"); */
            try
            {
                var wwwPath = _env.ContentRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", file);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}