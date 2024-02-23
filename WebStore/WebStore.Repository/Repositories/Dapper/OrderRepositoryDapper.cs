using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.Repository.Repositories.Dapper
{
    public class OrderRepositoryDapper : IOrderRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public OrderRepositoryDapper(IRelationalDatabaseConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersToBeShipped()
        {
            List<OrderModel> orders = new List<OrderModel>();
            List<AddressModel> addresses = new List<AddressModel>();
            List<OrderItemModel> orderItems = new List<OrderItemModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (var multi = await connection.QueryMultipleAsync("dbo.usp_GetOrdersToBeShipped", commandType: CommandType.StoredProcedure))
                {
                    orders = (List<OrderModel>)await multi.ReadAsync<OrderModel>();
                    orderItems = (List<OrderItemModel>)await multi.ReadAsync<OrderItemModel>();
                    addresses = (List<AddressModel>)await multi.ReadAsync<AddressModel>();

                    foreach (OrderModel order in orders)
                    {
                        order.OrderItems = orderItems.Where(x => x.OrderId == order.OrderId);
                        order.Address = addresses.First(x => x.AddressId == order.AddressId);
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
                using (var multi = await connection.QueryMultipleAsync("dbo.usp_GetOrdersWithOutstandingPayment", commandType: CommandType.StoredProcedure))
                {
                    orders = (List<OrderModel>)await multi.ReadAsync<OrderModel>();
                    orderItems = (List<OrderItemModel>)await multi.ReadAsync<OrderItemModel>();
                    addresses = (List<AddressModel>)await multi.ReadAsync<AddressModel>();

                    foreach (OrderModel order in orders)
                    {
                        order.OrderItems = orderItems.Where(x => x.OrderId == order.OrderId);
                        order.Address = addresses.First(x => x.AddressId == order.AddressId);
                    }
                }
            }
            return orders;
        }

        public async Task UpdateOrderPayment(int orderId, bool payed)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId, DbType.Int32);
            parameters.Add("@PaymentConfirmed", payed, DbType.Boolean);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                try
                {
                    await connection.ExecuteAsync("dbo.usp_UpdateCustomerOrderWithPayment", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw new Exception("Error updating payment confirmation");
                }
                
            }
        }

        public async Task UpdateOrderShipped(int orderId, bool shipped)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId, DbType.Int32);
            parameters.Add("@OrderShipped", shipped, DbType.Boolean);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                try
                {
                    await connection.ExecuteAsync("dbo.usp_UpdateCustomerOrderWithShipped", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw new Exception("Error updating shipped confirmation");
                }

            }
        }
    }
}
