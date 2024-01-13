using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IRegistrationService
    {
        Task<(bool IsSuccessful, string? Erorrs)> RegisterUserAsync(UserRegistrationDTO userRegistrationDTO);
    }
}
