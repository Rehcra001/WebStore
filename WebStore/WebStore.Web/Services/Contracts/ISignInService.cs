using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface ISignInService
    {
        Task<(bool IsSuccessful, string? jsonToken)> SignInAsync(UserSignInDTO userSignInDTO);
    }
}
