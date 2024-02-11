using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStore.Models;
using WebStore.Repository.Contracts;
using WebStore.Repository.Static;

namespace WebStore.Repository.Repositories.Dapper
{
    public class CustomerRepositoryDapper : ICustomerRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public CustomerRepositoryDapper(IRelationalDatabaseConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<AddressModel> AddAddress(AddressModel address, string email)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AddressLine1", address.AddressLine1, DbType.String);
            if (String.IsNullOrWhiteSpace(address.AddressLine2))
            {
                parameters.Add("@AddressLine2", DBNull.Value, DbType.String);
            }
            else
            {
                parameters.Add("@AddressLine2", address.AddressLine2, DbType.String);
            }            
            parameters.Add("@Suburb", address.Suburb, DbType.String);
            parameters.Add("@City", address.City, DbType.String);
            parameters.Add("@PostalCode", address.PostalCode, DbType.String);
            parameters.Add("@Country", address.Country, DbType.String);
            parameters.Add("@EmailAddress", email, DbType.String);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                address = await connection.QuerySingleAsync<AddressModel>("dbo.usp_AddCustomerAddress", parameters, commandType: CommandType.StoredProcedure);
            }

            return address;
        }

        public async Task<CustomerModel> AddCustomer(CustomerModel customer)
        {
            DataTable addressTable = Helper.CreateAddressesTable(customer.AddressList);

            string storedProcedure = "usp_AddCustomerWithAddresses";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@FirstName", customer.FirstName, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastName", customer.LastName, DbType.String, ParameterDirection.Input);
            parameters.Add("@EmailAddress", customer.EmailAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@PhoneNumber", customer.PhoneNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@Addresses", addressTable, DbType.Object, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                await connection.OpenAsync();
                var reader = await connection.ExecuteReaderAsync(storedProcedure, parameters);

                //On error a single row and column will be returned
                //The name of the column on error is Message
                if (reader.GetName(0).Equals("Message"))
                {
                    //error
                    await reader.ReadAsync();
                    string errorMessage = reader.GetString(reader.GetOrdinal("Message"));
                    string message = $"Repository: Unable to save the customer.\r\n\r\n{errorMessage}";
                    throw new Exception(message);
                }
                else
                {
                    if (reader.HasRows)
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

            return customer;
        }

        public async Task<OrderModel> AddOrder(int addressId, string email)
        {
            OrderModel order = new OrderModel();
            AddressModel address = new AddressModel();
            List<OrderItemModel> orderItems = new List<OrderItemModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmailAddress", email, DbType.String);
            parameters.Add("@AddressId", addressId, DbType.Int32);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (var multiResultSets = await connection.QueryMultipleAsync("dbo.usp_AddOrder", parameters, commandType: CommandType.StoredProcedure))
                {
                    order = await multiResultSets.ReadSingleAsync<OrderModel>();
                    address = await multiResultSets.ReadSingleAsync<AddressModel>();
                    orderItems = (List<OrderItemModel>)await multiResultSets.ReadAsync<OrderItemModel>();
                }

                order.Address = address;
                order.OrderItems = orderItems;
            }
            return order;
        }

        public async Task<AddressModel> GetAddressById(int addressId, string email)
        {
            AddressModel address = new AddressModel();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AddressId", addressId, DbType.Int32);
            parameters.Add("@EmailAddress", email, DbType.String);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                address = await connection.QuerySingleAsync<AddressModel>("dbo.usp_GetAddressWithId", parameters, commandType: CommandType.StoredProcedure);
            }
            return address;
        }

        public async Task<IEnumerable<AddressLineModel>> GetAddressLines(string email)
        {
            List<AddressLineModel> addressLines = new List<AddressLineModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmailAddress", email, DbType.String);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                var returned = (List<AddressLineModel>)await connection.QueryAsync<AddressLineModel>("dbo.usp_GetAddressLines", parameters, commandType: CommandType.StoredProcedure);

                if (returned != null && returned.Count > 0)
                {
                    addressLines = returned;
                }
            }

            return addressLines;
        }

        public async Task<CustomerModel> GetCustomer(string email)
        {
            CustomerModel customer = new CustomerModel();
            string storedProcedure = "dbo.usp_GetCustomer";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmailAddress", email, DbType.String, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (var multi = await connection.QueryMultipleAsync(storedProcedure, parameters))
                {
                    customer = await multi.ReadSingleAsync<CustomerModel>();
                    var addresses = await multi.ReadAsync<AddressModel>();

                    customer.AddressList = addresses;
                }
            }
            return customer;
        }

        public Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public Task<CustomerModel> UpdateCustomer(CustomerModel customer)
        {
            throw new NotImplementedException();
        }
    }
}
