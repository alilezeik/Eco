namespace Eco
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class EmailManager
    {
        private readonly IEmailService emailService;

        public EmailManager(IEmailService emailService)
        {
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<bool> SendEmail(Email email)
        {
           return await this.emailService.SendEmail(email);
        }

    }
}
