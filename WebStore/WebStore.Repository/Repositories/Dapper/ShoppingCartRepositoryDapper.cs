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

        public async Task DeleteCartItem(int id, string emailAddress)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CartItemId", id, DbType.Int32);
            parameters.Add("@EmailAddress", emailAddress, DbType.String);

            try
            {
                using (SqlConnection connection = _sqlConnection.SqlConnection())
                {
                    await connection.ExecuteAsync("dbo.usp_DeleteCartItem", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
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


        public async Task<CartItemModel> UpdateCartItemQuantity(CartItemModel cartItem, string emailAddress)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CartItemId", cartItem.CartItemId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Quantity", cartItem.Quantity, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmailAddress", emailAddress, DbType.String, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                cartItem = await connection.QuerySingleAsync<CartItemModel>("dbo.usp_UpdateCartItemQuantity", parameters, commandType: CommandType.StoredProcedure);                
            }
            return cartItem;
        }
    }
}
