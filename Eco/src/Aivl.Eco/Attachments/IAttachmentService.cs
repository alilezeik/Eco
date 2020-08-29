namespace Eco
{
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAttachmentService
    {
        Task<string> Upload(IFormFile file);

        Task<bool> Delete(string url);
    }
}
