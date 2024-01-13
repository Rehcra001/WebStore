using Microsoft.Data.SqlClient;
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
                            if (reader.HasRows)
                            {
                                customer.AddressList = new List<AddressModel>();

                                while (await reader.ReadAsync())
                                {
                                    AddressModel address = new AddressModel();

                                    address.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                                    address.AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1"));
                                    address.AddressLine2 = reader.GetString(reader.GetOrdinal("AddressLine2"));
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
