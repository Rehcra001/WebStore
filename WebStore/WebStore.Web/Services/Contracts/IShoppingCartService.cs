﻿using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<CartItemDTO> AddCartItem(CartItemAddToDTO cartItemAddToDTO);
        Task<IEnumerable<CartItemDTO>> GetCartItems();
    }
}
