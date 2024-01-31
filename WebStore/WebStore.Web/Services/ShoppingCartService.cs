using Blazored.LocalStorage;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public ShoppingCartService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<CartItemDTO> AddCartItem(CartItemDTO cartItemDTO)
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);


                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<CartItemDTO>("api/shoppingcart/addcartitem", cartItemDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(CartItemDTO);
                    }
                    else
                    {
                        cartItemDTO = await httpResponseMessage.Content.ReadFromJsonAsync<CartItemDTO>();
                        return cartItemDTO;
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
    }
}
