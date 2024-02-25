using Microsoft.Data.SqlClient;
using System.Data;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.Repository.Repositories.ADO
{
    public class OrderRepositoryADO : IOrderRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public OrderRepositoryADO(IRelationalDatabaseConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<OrderModel> GetOrderById(int id)
        {
            OrderModel order = new OrderModel();
            AddressModel address = new AddressModel();
            List<OrderItemModel> orderItems = new List<OrderItemModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetOrderById";
                    command.Parameters.Add("@OrderId", SqlDbType.Int).Value = id;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            //Read in Order header data
                            await reader.ReadAsync();

                            order.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                            order.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                            order.EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress"));
                            order.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            order.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                            order.OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"));
                            order.TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"));
                            order.PaymentConfirmed = reader.GetBoolean(reader.GetOrdinal("PaymentConfirmed"));
                            order.OrderShipped = reader.GetBoolean(reader.GetOrdinal("OrderShipped"));
                            order.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));

                            await reader.NextResultAsync();

                            //Read in order items
                            while (await reader.ReadAsync())
                            {
                                OrderItemModel orderItem = new OrderItemModel();
                                orderItem.OrderItemId = reader.GetInt32(reader.GetOrdinal("OrderItemId"));
                                orderItem.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                                orderItem.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"));
                                orderItem.ProductName = reader.GetString(reader.GetOrdinal("ProductName"));
                                orderItem.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                                orderItem.Price = reader.GetDecimal(reader.GetOrdinal("Price"));

                                orderItems.Add(orderItem);
                            }
                            await reader.NextResultAsync();

                            //Read in ship to address
                            await reader.ReadAsync();
                            address.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                            address.AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1"));
                            if (!reader.IsDBNull(reader.GetOrdinal("AddressLine2")))
                            {
                                address.AddressLine2 = reader.GetString(reader.GetOrdinal("AddressLine2"));
                            }
                            address.Suburb = reader.GetString(reader.GetOrdinal("Suburb"));
                            address.City = reader.GetString(reader.GetOrdinal("City"));
                            address.PostalCode = reader.GetString(reader.GetOrdinal("PostalCode"));
                            address.Country = reader.GetString(reader.GetOrdinal("Country"));
                            address.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));

                            //Combine all
                            order.OrderItems = orderItems;
                            order.Address = address;
                        }
                    }
                }
            }
            return order;
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersToBeShipped()
        {
            List<OrderModel> orders = new List<OrderModel>();
            List<AddressModel> addresses = new List<AddressModel>();
            List<OrderItemModel> orderItems = new List<OrderItemModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetOrdersToBeShipped";

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            //Read in Order header data
                            while (await reader.ReadAsync())
                            {
                                OrderModel order = new OrderModel();
                                order.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                                order.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                                order.EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress"));
                                order.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                order.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                                order.OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"));
                                order.TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"));
                                order.PaymentConfirmed = reader.GetBoolean(reader.GetOrdinal("PaymentConfirmed"));
                                order.OrderShipped = reader.GetBoolean(reader.GetOrdinal("OrderShipped"));
                                order.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));

                                orders.Add(order);
                            }
                            await reader.NextResultAsync();

                            //Read in order items
                            while (await reader.ReadAsync())
                            {
                                OrderItemModel orderItem = new OrderItemModel();
                                orderItem.OrderItemId = reader.GetInt32(reader.GetOrdinal("OrderItemId"));
                                orderItem.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                                orderItem.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"));
                                orderItem.ProductName = reader.GetString(reader.GetOrdinal("ProductName"));
                                orderItem.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                                orderItem.Price = reader.GetDecimal(reader.GetOrdinal("Price"));

                                orderItems.Add(orderItem);
                            }
                            await reader.NextResultAsync();

                            //Read in ship to address
                            while (await reader.ReadAsync())
                            {
                                AddressModel address = new AddressModel();
                                address.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                                address.AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1"));
                                if (!reader.IsDBNull(reader.GetOrdinal("AddressLine2")))
                                {
                                    address.AddressLine2 = reader.GetString(reader.GetOrdinal("AddressLine2"));
                                }
                                address.Suburb = reader.GetString(reader.GetOrdinal("Suburb"));
                                address.City = reader.GetString(reader.GetOrdinal("City"));
                                address.PostalCode = reader.GetString(reader.GetOrdinal("PostalCode"));
                                address.Country = reader.GetString(reader.GetOrdinal("Country"));
                                address.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));

                                addresses.Add(address);
                            }

                            //Combine all
                            foreach (OrderModel order in orders)
                            {
                                order.Address = addresses.First(x => x.AddressId == order.AddressId);
                                order.OrderItems = orderItems.Where(x => x.OrderId == order.OrderId);
                            }
                        }
                    }
                }
            }
            return orders;
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersWithOutstandingPayment()
        {
            List<OrderModel> orders = new List<OrderModel>();
            List<AddressModel> addresses = new List<AddressModel>();
            List<OrderItemModel> orderItems = new List<OrderItemModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetOrdersWithOutstandingPayment";

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            //Read in Order header data
                            while (await reader.ReadAsync())
                            {
                                OrderModel order = new OrderModel();
                                order.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                                order.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                                order.EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress"));
                                order.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                order.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                                order.OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"));
                                order.TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"));
                                order.PaymentConfirmed = reader.GetBoolean(reader.GetOrdinal("PaymentConfirmed"));
                                order.OrderShipped = reader.GetBoolean(reader.GetOrdinal("OrderShipped"));
                                order.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));

                                orders.Add(order);
                            }
                            await reader.NextResultAsync();

                            //Read in order items
                            while (await reader.ReadAsync())
                            {
                                OrderItemModel orderItem = new OrderItemModel();
                                orderItem.OrderItemId = reader.GetInt32(reader.GetOrdinal("OrderItemId"));
                                orderItem.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                                orderItem.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"));
                                orderItem.ProductName = reader.GetString(reader.GetOrdinal("ProductName"));
                                orderItem.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                                orderItem.Price = reader.GetDecimal(reader.GetOrdinal("Price"));

                                orderItems.Add(orderItem);
                            }
                            await reader.NextResultAsync();

                            //Read in ship to address
                            while (await reader.ReadAsync())
                            {
                                AddressModel address = new AddressModel();
                                address.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                                address.AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1"));
                                if (!reader.IsDBNull(reader.GetOrdinal("AddressLine2")))
                                {
                                    address.AddressLine2 = reader.GetString(reader.GetOrdinal("AddressLine2"));
                                }
                                address.Suburb = reader.GetString(reader.GetOrdinal("Suburb"));
                                address.City = reader.GetString(reader.GetOrdinal("City"));
                                address.PostalCode = reader.GetString(reader.GetOrdinal("PostalCode"));
                                address.Country = reader.GetString(reader.GetOrdinal("Country"));
                                address.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));

                                addresses.Add(address);
                            }

                            //Combine all
                            foreach (OrderModel order in orders)
                            {
                                order.Address = addresses.First(x => x.AddressId == order.AddressId);
                                order.OrderItems = orderItems.Where(x => x.OrderId == order.OrderId);
                            }
                        }
                    }
                }
            }
            return orders;
        }

        public async Task UpdateOrderPayment(int orderId, bool payed)
        {
            try
            {
                using (SqlConnection connection = _sqlConnection.SqlConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.usp_UpdateCustomerOrderWithPayment";
                        command.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;
                        command.Parameters.Add("@PaymentConfirmed", SqlDbType.Bit).Value = payed;

                        await command.Connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error updating payment confirmation");
            }
        }

        public async Task UpdateOrderShipped(int orderId, bool shipped)
        {
            try
            {
                using (SqlConnection connection = _sqlConnection.SqlConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.usp_UpdateCustomerOrderWithShipped";
                        command.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;
                        command.Parameters.Add("@OrderShipped", SqlDbType.Bit).Value = shipped;

                        await command.Connection.OpenAsync();

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error updating shipped confirmation");
            }
        }
    }
}
