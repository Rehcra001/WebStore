using WebStore.API.Services.Contracts;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.API.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository  _shoppingCartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<CartItemModel> AddCartItem(CartItemModel cartItem, string emailAddress)
        {
            try
            {
                return await _shoppingCartRepository.AddCartItem(cartItem, emailAddress);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<CartItemModel> GetCartItems(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public Task<CartItemModel> UpdateCartItemQuantity(int quantity, string emailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
