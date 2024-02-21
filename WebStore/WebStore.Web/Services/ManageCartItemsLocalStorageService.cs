using Blazored.LocalStorage;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IShoppingCartService _shoppingCartService;
        private const string KEY = "CartItemCollection";
        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService)
        {
            _localStorageService = localStorageService;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IEnumerable<CartItemDTO>> GetCollection()
        {
            return await _localStorageService.GetItemAsync<IEnumerable<CartItemDTO>>(KEY) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(KEY);
        }

        public async Task SaveCollection(List<CartItemDTO> cartItemDTOs)
        {
            await _localStorageService.SetItemAsync(KEY, cartItemDTOs);
        }

        private async Task<IEnumerable<CartItemDTO>> AddCollection()
        {
            var cartItemsCollection = await _shoppingCartService.GetCartItems();
            if (cartItemsCollection != null)
            {
                await _localStorageService.SetItemAsync(KEY, cartItemsCollection);
            }
            return cartItemsCollection;
        }
    }
}
