using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.Repository.Repositories.Dapper
{
    public class ShoppingCartRepositoryDapper : IShoppingCartRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public ShoppingCartRepositoryDapper(IRelationalDatabaseConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<CartItemModel> AddCartItem(CartItemModel cartItem, string emailAddress)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmailAddress", emailAddress, dbType: DbType.String, ParameterDirection.Input);
            parameters.Add("@ProductId", cartItem.ProductId, dbType: DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Quantity", cartItem.Quantity, dbType: DbType.Int32, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                cartItem = await connection.QuerySingleAsync<CartItemModel>("usp_AddCartItem", parameters, commandType: CommandType.StoredProcedure);
            }

            return cartItem;
        }

        public async Task<IEnumerable<CartItemModel>> GetCartItems(string emailAddress)
        {
            List<CartItemModel> cartItems = new List<CartItemModel>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmailAddress", emailAddress, DbType.String);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                var returned = await connection.QueryAsync<IEnumerable<CartItemModel>>("usp_GetCartItems", parameters, commandType: CommandType.StoredProcedure);

                if (returned != null)
                {
                    cartItems = (List<CartItemModel>)returned;
                }
                
            }

            return cartItems;
        }

        public Task<CartItemModel> UpdateCartItemQuantity(int quantity, string emailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
