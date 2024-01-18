using System.Net;
using System.Net.Http.Json;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDTO> AddProductAsync(ProductDTO productDTO)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<ProductDTO>("api/product/addproduct", productDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(ProductDTO);
                    }
                    else
                    {
                        ProductDTO product = await httpResponseMessage.Content.ReadFromJsonAsync<ProductDTO>();
                        return product;
                    }
                }
                else
                {
                    var message = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
