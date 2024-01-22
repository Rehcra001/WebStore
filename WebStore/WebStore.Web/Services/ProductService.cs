﻿using System.Net;
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

        public async Task<IEnumerable<ProductCategoryDTO>> GetProductCategories()
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
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<UnitPerDTO>> GetUnitPers()
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
