﻿using Dapper;
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

            public Task<CustomerModel> GetCustomer(int id)
        {
            throw new NotImplementedException();
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