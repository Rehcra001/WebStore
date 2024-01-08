using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IRegistrationService
    {
        Task<(bool, string?)> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO);
    }
}
