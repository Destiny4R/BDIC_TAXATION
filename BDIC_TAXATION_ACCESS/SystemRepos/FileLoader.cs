using Microsoft.AspNetCore.Http;

namespace BDIC_TAXATION_ACCESS.SystemRepos
{
    public class FileLoader : IFileLoader
    {
        public async Task<string> Fileupload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "certificatefiles", fileName);
                //Check if the directory exists, if not create it
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return "/certificatefiles/" + fileName;
            }
            return null;
        }
    }
}
