namespace Eco
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AttachmentManager
    {
        private readonly IAttachmentService attachmentService;

        public AttachmentManager(IAttachmentService attachmentService)
        {
            this.attachmentService = attachmentService ?? throw new ArgumentNullException(nameof(attachmentService));
        }

        public async Task<string> Upload(IFormFile file)
        {
           return await this.attachmentService.Upload(file);
        }

        public async Task<bool> Delete(string url)
        {
            return await this.attachmentService.Delete(url);
        }
    }
}
