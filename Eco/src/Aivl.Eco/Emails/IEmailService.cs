namespace Eco
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
