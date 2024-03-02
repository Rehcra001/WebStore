using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Data;
using WebStore.Models;
using WebStore.Repository.Contracts;
using WebStore.Repository.Static;

namespace WebStore.Repository.Repositories.ADO
{
    public class CustomerRepositoryADO : ICustomerRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public CustomerRepositoryADO(IRelationalDatabaseConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<AddressModel> AddAddress(AddressModel address, string email)
        {
            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_AddCustomerAddress";
                    command.Parameters.Add("@AddressLine1", SqlDbType.NVarChar).Value = address.AddressLine1;
                    if (String.IsNullOrWhiteSpace(address.AddressLine2))
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = address.AddressLine2;
                    }
                    command.Parameters.Add("@Suburb", SqlDbType.NVarChar).Value = address.Suburb;
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = address.City;
                    command.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = address.PostalCode;
                    command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = address.Country;
                    command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email;

                    await command.Connection.OpenAsync();

                    var id = await command.ExecuteScalarAsync();

                    if (int.TryParse(id.ToString(), out _))
                    {
                        address.AddressId = int.Parse(id.ToString());
                    }
                    else
                    {
                        address = new AddressModel();
                    }
                }
            }
            return address;
        }

        public async Task<CustomerModel> AddCustomer(CustomerModel customer)
        {
            try
            {
                DataTable addressesTable = Helper.CreateAddressesTable(customer.AddressList);

                using (SqlConnection connection = _sqlConnection.SqlConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.usp_AddCustomerWithAddresses";
                        command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = customer.FirstName;
                        command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = customer.LastName;
                        command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = customer.EmailAddress;
                        command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = customer.PhoneNumber;
                        command.Parameters.Add("@Addresses", SqlDbType.Structured).Value = addressesTable;

                        await command.Connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                //check for errors
                                //expecting one row and one column with name of "Message" on error

                                if (reader.GetName(0).Equals("Message"))
                                {
                                    await reader.ReadAsync();
                                    string errorMessage = reader.GetString(reader.GetOrdinal("Message"));
                                    string message = $"Repository: Unable to save the customer.\r\n\r\n{errorMessage}";
                                    throw new Exception(message);
                                }
                                else
                                {
                                    List<AddressModel> addresses = new List<AddressModel>();

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
                                    //retrieve the customer id from the first address
                                    customer.CustomerId = addresses[0].CustomerId;
                                    customer.AddressList = addresses;
                                }
                            }
                        }
                    }
                }
                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderModel> AddOrder(int addressId, string email)
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
                    command.CommandText = "dbo.usp_AddOrder";
                    command.Parameters.Add("@AddressId", SqlDbType.Int).Value = addressId;
                    command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            //get order data
                            while (await reader.ReadAsync())
                            {
                                order.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                                order.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                                order.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                order.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                                order.OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"));
                                order.TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"));
                                order.PaymentConfirmed = reader.GetBoolean(reader.GetOrdinal("PaymentConfirmed"));
                                order.OrderShipped = reader.GetBoolean(reader.GetOrdinal("OrderShipped"));
                                order.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                            }
                            //get address data
                            await reader.NextResultAsync();
                            await reader.ReadAsync();

                            address.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                            address.AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1"));
                            if (!reader.IsDBNull(reader.GetOrdinal("AddressLine2")))
                            {
                                address.AddressLine2 = reader.GetString(reader.GetOrdinal("AddressLine2"));
                            }
                            address.Suburb = reader.GetString(reader.GetOrdinal("Suburb)"));
                            address.City = reader.GetString(reader.GetOrdinal("City"));
                            address.PostalCode = reader.GetString(reader.GetOrdinal("PostalCode"));
                            address.Country = reader.GetString(reader.GetOrdinal("Country"));
                            address.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));

                            order.Address = address;
                            //get order item data
                            await reader.NextResultAsync();
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
                            order.OrderItems = orderItems;
                        }
                    }
                }
            }
            return order;
        }

        public async Task<AddressModel> GetAddressById(int addressId, string email)
        {
            AddressModel address = new AddressModel();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAddressWithId";
                    command.Parameters.Add("@AddressId", SqlDbType.Int).Value = addressId;
                    command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
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
                            }
                        }
                    }
                }
            }
            return address;
        }

        public async Task<IEnumerable<AddressLineModel>> GetAddressLines(string email)
        {
            List<AddressLineModel> addressLines = new List<AddressLineModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetAddressLines";
                    command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                AddressLineModel addressLine = new AddressLineModel()
                                {
                                    AddressId = reader.GetInt32(reader.GetOrdinal("AddressId")),
                                    AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1"))
                                };

                                addressLines.Add(addressLine);
                            }
                        }
                    }
                }
            }
            return addressLines;
        }

        public async Task<CustomerModel> GetCustomer(string email)
        {
            CustomerModel customer = new CustomerModel();
            List<AddressModel> addresses = new List<AddressModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetCustomer";
                    command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {

                            //Read in the customer
                            await reader.ReadAsync();
                            customer.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                            customer.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            customer.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                            customer.EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress"));
                            customer.PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"));

                            //Read in the customer address
                            await reader.NextResultAsync();
                            customer.AddressList = new List<AddressModel>();

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
                            customer.AddressList = addresses;
                        }
                    }
                }
            }
            return customer;
        }

        public async Task<IEnumerable<OrderModel>> GetCustomerOrders(string email)
        {
            List<OrderModel> orders = new List<OrderModel>();
            List<OrderItemModel> orderItems = new List<OrderItemModel>();
            List<AddressModel> addresses = new List<AddressModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "usp_GetCustomerOrders";
                    command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = email;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            // Get orders
                            while (await reader.ReadAsync())
                            {
                                OrderModel order = new OrderModel();

                                order.OrderId = reader.GetInt32(reader.GetOrdinal("OrderId"));
                                order.CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                                order.OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"));
                                order.TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice"));
                                order.PaymentConfirmed = reader.GetBoolean(reader.GetOrdinal("PaymentConfirmed"));
                                order.OrderShipped = reader.GetBoolean(reader.GetOrdinal("OrderShipped"));
                                order.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));

                                orders.Add(order);
                            }

                            // Get order items
                            await reader.NextResultAsync();
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

                            // Get ship addresses
                            await reader.NextResultAsync();
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

                            foreach (OrderModel order in orders)
                            {
                                order.OrderItems = orderItems.Where(x => x.OrderId == order.OrderId);
                                order.Address = addresses.First(x => x.AddressId == order.AddressId);
                            }
                        }
                    }
                }
            }
            return orders;
        }

        public async Task<bool> UpdateCustomerAddress(AddressModel address)
        {
            bool isUpdated = false;

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_updateCustomerAddress";
                    command.Parameters.Add("@AddressId", SqlDbType.Int).Value = address.AddressId;
                    command.Parameters.Add("@AddressLine1", SqlDbType.NVarChar).Value = address.AddressLine1;
                    if (String.IsNullOrWhiteSpace(address.AddressLine2))
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = DBNull.Value;
                    }
                    else
                    {
                        command.Parameters.Add("@AddressLine2", SqlDbType.NVarChar).Value = address.AddressLine2;
                    }

                    command.Parameters.Add("@Suburb", SqlDbType.NVarChar).Value = address.Suburb;
                    command.Parameters.Add("@City", SqlDbType.NVarChar).Value = address.City;
                    command.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = address.PostalCode;
                    command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = address.Country;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();
                            isUpdated = reader.GetBoolean(reader.GetOrdinal("Response"));
                        }
                    }
                }
            }
            return isUpdated;
        }

        public async Task<bool> UpdateCustomerDetail(CustomerModel customer)
        {
            bool isUpdated = false;

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_UpdateCustomerDetail";
                    command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customer.CustomerId;
                    command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = customer.FirstName;
                    command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = customer.LastName;
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = customer.PhoneNumber;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();
                            isUpdated = reader.GetBoolean(reader.GetOrdinal("Response"));
                        }
                    }

                }
            }

            return isUpdated;
        }
    }
}
