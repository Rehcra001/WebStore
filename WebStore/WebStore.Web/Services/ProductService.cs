using System.Net;
using System.Net.Http.Json;
using System.Security.AccessControl;
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

        public async Task<ProductCategoryDTO> AddCategoryAsync(ProductCategoryDTO productCategoryDTO)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<ProductCategoryDTO>("api/product/AddProductCategory", productCategoryDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(ProductCategoryDTO);
                    }
                    else
                    {
                        productCategoryDTO = await httpResponseMessage.Content.ReadFromJsonAsync<ProductCategoryDTO>();
                        return productCategoryDTO;
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
                    throw new Exception($"Http status: {httpResponseMessage.StatusCode} Message -{message}");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UnitPerDTO> AddUnitPerAsync(UnitPerDTO unitPerDTO)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync<UnitPerDTO>("api/product/AddUnitPer", unitPerDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(UnitPerDTO);
                    }
                    else
                    {
                        unitPerDTO = await httpResponseMessage.Content.ReadFromJsonAsync<UnitPerDTO>();
                        return unitPerDTO;
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

        public async Task<IEnumerable<ProductCategoryDTO>> GetProductCategoriesAsync()
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/product/getproductcategories");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductCategoryDTO>();
                    }
                    else
                    {
                        IEnumerable<ProductCategoryDTO> productCategories = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDTO>>();

                        return productCategories;
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

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/product/getproducts");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDTO>();
                    }
                    else
                    {
                        IEnumerable<ProductDTO> products = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();
                        return products;
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

        public async Task<IEnumerable<UnitPerDTO>> GetUnitPersAsync()
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/product/getunitpers");

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<UnitPerDTO>();
                    }
                    else
                    {
                        IEnumerable<UnitPerDTO> unitPers = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<UnitPerDTO>>();
                        return unitPers;
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

        public async Task<ProductDTO> UpdateProductAsync(ProductDTO productDTO)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync("api/product/UpdateProduct", productDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        var message = await httpResponseMessage.Content.ReadAsStringAsync();
                        throw new Exception($"Http status: {httpResponseMessage.StatusCode} Message -{message}");
                    }
                    else
                    {
                        productDTO = await httpResponseMessage.Content.ReadFromJsonAsync<ProductDTO>();
                        return productDTO;

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

        public async Task<ProductCategoryDTO> UpdateProductCategoryAsync(ProductCategoryDTO productCategoryDTO)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync<ProductCategoryDTO>("api/product/updateproductcategory", productCategoryDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(ProductCategoryDTO);
                    }
                    else
                    {
                        productCategoryDTO = await httpResponseMessage.Content.ReadFromJsonAsync<ProductCategoryDTO>();
                        return productCategoryDTO;
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

        public async Task<UnitPerDTO> UpdateUnitPerAsync(UnitPerDTO unitPerDTO)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync<UnitPerDTO>("api/product/updateunitper", unitPerDTO);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    if (httpResponseMessage.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(UnitPerDTO);
                    }
                    else
                    {
                        unitPerDTO = await httpResponseMessage.Content.ReadFromJsonAsync<UnitPerDTO>();
                        return unitPerDTO;
                    }
                }
                else
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
    }
}
