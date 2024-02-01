using Microsoft.Data.SqlClient;
using System.Data;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.Repository.Repositories.ADO
{
    public class ShoppingCartRepositoryADO : IShoppingCartRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public ShoppingCartRepositoryADO(IRelationalDatabaseConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<CartItemModel> AddCartItem(CartItemModel cartItem, string emailAddress)
        {
            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_AddCartItem";
                    command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = emailAddress;
                    command.Parameters.Add("@ProductId", SqlDbType.Int).Value = cartItem.ProductId;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = cartItem.Quantity;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();

                            cartItem.CartItemId = reader.GetInt32(reader.GetOrdinal("CartItemId"));
                            cartItem.CartId = reader.GetInt32(reader.GetOrdinal("CartId"));
                        }
                    }
                }
            }
            return cartItem;
        }

        public async Task<IEnumerable<CartItemModel>> GetCartItems(string emailAddress)
        {
            List<CartItemModel> cartItems = new List<CartItemModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "usp_GetCartItems";
                    command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = emailAddress;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                CartItemModel cartItem = new CartItemModel()
                                {
                                    CartItemId = reader.GetInt32(reader.GetOrdinal("CartItemId")),
                                    CartId = reader.GetInt32(reader.GetOrdinal("CartId")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                                };

                                cartItems.Add(cartItem);
                            }
                        }
                    }
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
