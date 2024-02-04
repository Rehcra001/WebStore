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
        public event Action<int> OnShoppingCartChanged;

        public ShoppingCartService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<CartItemDTO> AddCartItem(CartItemAddToDTO cartItemAddToDTO)
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);


                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<CartItemAddToDTO>("api/shoppingcart/addcartitem", cartItemAddToDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(CartItemDTO);
                    }
                    else
                    {
                        CartItemDTO cartItemDTO = await httpResponseMessage.Content.ReadFromJsonAsync<CartItemDTO>();
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

        public async Task DeleteCartItem(int id)
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"api/shoppingcart/deletecartitem/{id}");

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception($"Http Status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CartItemDTO>> GetCartItems()
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/shoppingcart/getcartitems");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    //check for content
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDTO>();
                    }
                    else
                    {
                        IEnumerable<CartItemDTO> cartItemDTOs = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<CartItemDTO>>();
                        return cartItemDTOs;
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

        public void RaiseShoppingCartChangedEvent(int totalQty)
        {
            OnShoppingCartChanged?.Invoke(totalQty);
        }

        public async Task<CartItemDTO> UpdateCartItemQuantity(UpdateCartItemQuantityDTO updateCartItemQuantityDTO)
        {
            try
            {
                var jsonToken = await _localStorageService.GetItemAsync<string>("bearerToken");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jsonToken);

                HttpResponseMessage httpResponseMessage = await _httpClient.PatchAsJsonAsync<UpdateCartItemQuantityDTO>($"api/shoppingcart/updatecartitemquantity/{updateCartItemQuantityDTO.CartItemId}", updateCartItemQuantityDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(CartItemDTO);
                    }
                    return await httpResponseMessage.Content.ReadFromJsonAsync<CartItemDTO>();
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
