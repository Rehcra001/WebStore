using Blazored.LocalStorage;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        public CustomerService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<CustomerDTO> GetCustomerDetailsAsync()
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"api/customer");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    //Check for content
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(CustomerDTO);
                    }

                    CustomerDTO customerDTO = await httpResponseMessage.Content.ReadFromJsonAsync<CustomerDTO>();
                    return customerDTO;
                }
                else
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception(message);
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
