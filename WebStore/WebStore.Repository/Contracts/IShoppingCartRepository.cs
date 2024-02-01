using WebStore.Models;

namespace WebStore.Repository.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<CartItemModel> AddCartItem(CartItemModel cartItem, string emailAddress);
        Task<IEnumerable<CartItemModel>> GetCartItems(string emailAddress);
        Task<CartItemModel> UpdateCartItemQuantity(int quantity, string emailAddress);        
    }
}
