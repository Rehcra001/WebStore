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

        public async Task DeleteCartItem(int id, string emailAddress)
        {
            try
            {
                await _shoppingCartRepository.DeleteCartItem(id, emailAddress);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CartItemModel>> GetCartItems(string emailAddress)
        {
            try
            {
                return await _shoppingCartRepository.GetCartItems(emailAddress);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CartItemModel> UpdateCartItemQuantity(CartItemModel cartItem, string emailAddress)
        {
            try
            {
                return await _shoppingCartRepository.UpdateCartItemQuantity(cartItem, emailAddress);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
