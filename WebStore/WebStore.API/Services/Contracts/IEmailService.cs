using WebStore.DTO;

namespace WebStore.API.Services.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDTO request);
    }
}
