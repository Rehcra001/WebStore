using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.Repository.Repositories.Dapper
{
    public class ShoppingCartRepositoryDapper : IShoppingCartRepository
    {
        public Task<CartItemModel> AddCartItem(CartItemModel cartItem, string emailAddress)
        {
            throw new NotImplementedException();
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
