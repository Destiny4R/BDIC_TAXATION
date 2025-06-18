using Microsoft.AspNetCore.Http;

namespace BDIC_TAXATION_ACCESS.SystemRepos
{
    public interface IFileLoader
    {
        Task<string> Fileupload(IFormFile file);
    }
}
