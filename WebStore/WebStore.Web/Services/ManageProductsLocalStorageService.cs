using Blazored.LocalStorage;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IProductService _productService;

        private const string KEY = "ProductCollection";
        public ManageProductsLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
        {
            _localStorageService = localStorageService;
            _productService = productService;
        }

        public async Task<IEnumerable<ProductDTO>> GetCollection()
        {
            return await _localStorageService.GetItemAsync<IEnumerable<ProductDTO>>(KEY) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(KEY);
        }

        public async Task SaveCollection(List<ProductDTO> products)
        {
            await _localStorageService.SetItemAsync(KEY, products);
        }

        private async Task<IEnumerable<ProductDTO>> AddCollection()
        {
            var productCollection = await _productService.GetProductsAsync();

            if (productCollection != null)
            {
                await _localStorageService.SetItemAsync(KEY, productCollection);
            }

            return productCollection;
        }        
    }
}
