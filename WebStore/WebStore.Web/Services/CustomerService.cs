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

        public async Task<AddressDTO> AddCustomerAddress(AddressDTO addressDTO)
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<AddressDTO>("api/customer/addcustomeraddress", addressDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(AddressDTO);
                    }
                    else
                    {
                        addressDTO = await httpResponseMessage.Content.ReadFromJsonAsync<AddressDTO>();
                        return addressDTO;
                    }
                }
                else
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<AddressLineDTO>> GetAddresLinesAsync()
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/customer/getaddresslines");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<AddressLineDTO>();
                    }
                    else
                    {
                        IEnumerable<AddressLineDTO> addressLines = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<AddressLineDTO>>();
                        return addressLines;
                    }
                }
                else
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
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
                    throw new Exception($"Http status: {httpResponseMessage.StatusCode} Message -{message}");
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
