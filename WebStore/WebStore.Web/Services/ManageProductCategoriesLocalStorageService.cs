using Blazored.LocalStorage;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class ManageProductCategoriesLocalStorageService : IManageProductCategoriesLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IProductService _productService;
        private const string KEY = "ProductCategoriesCollection";

        public ManageProductCategoriesLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
        {
            _localStorageService = localStorageService;
            _productService = productService;
        }

        public async Task<IEnumerable<ProductCategoryDTO>> GetCollection()
        {
            return await _localStorageService.GetItemAsync<IEnumerable<ProductCategoryDTO>>(KEY) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(KEY);
        }

        private async Task<IEnumerable<ProductCategoryDTO>> AddCollection()
        {
            var productCategoryCollection = await _productService.GetProductCategoriesAsync();
            if (productCategoryCollection != null)
            {
                await _localStorageService.SetItemAsync(KEY, productCategoryCollection);
            }
            return productCategoryCollection;
        }
    }
}
