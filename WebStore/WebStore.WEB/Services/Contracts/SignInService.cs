using System.Net.Http.Json;
using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public class SignInService : ISignInService
    {
        private readonly HttpClient _httpClient;

        public SignInService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool IsSuccessful, string? jsonToken)> SignIn(UserSignInDTO userSignInDTO)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("api/user/signin", userSignInDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonWebToken = await httpResponseMessage.Content.ReadAsStringAsync();

                    return (true, jsonWebToken);
                }
                else
                {
                    return (false, null);
                }
            }
            catch (Exception)
            {
                //Log Exception
                throw;
            }
        }
    }
}
