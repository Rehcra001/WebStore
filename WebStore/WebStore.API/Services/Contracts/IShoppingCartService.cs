using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartItemModel> AddCartItem(CartItemModel cartItem, string emailAddress);
        Task<CartItemModel> GetCartItems(string emailAddress);
        Task<CartItemModel> UpdateCartItemQuantity(int quantity, string emailAddress);
    }
}
