using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartItemDTO> AddCartItem(CartItemDTO cartItemDTO);
    }
}
