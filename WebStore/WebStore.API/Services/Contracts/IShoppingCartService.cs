using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartItemModel> AddCartItem(CartItemModel cartItem, string emailAddress);
        Task<IEnumerable<CartItemModel>> GetCartItems(string emailAddress);
        Task<CartItemModel> UpdateCartItemQuantity(CartItemModel cartItem, string emailAddress);
        Task DeleteCartItem(int id, string emailAddress);
    }
}
