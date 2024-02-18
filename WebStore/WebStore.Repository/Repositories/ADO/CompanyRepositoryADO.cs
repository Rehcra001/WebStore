using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using WebStore.Models;
using WebStore.Repository.Contracts;
using WebStore.Repository.Static;

namespace WebStore.Repository.Repositories.ADO
{
    public class CompanyRepositoryADO : ICompanyRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public CompanyRepositoryADO(IRelationalDatabaseConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public async Task<(CompanyDetailModel CompanyDetailModel, CompanyEFTDetailModel CompanyEFTDetailModel, AddressModel CompanyAddressModel)> AddCompanyDetail(CompanyDetailModel companyDetail, CompanyEFTDetailModel companyEFTDetail, AddressModel companyAddress)
        {
            List<AddressModel> addresses = new List<AddressModel>()
            {
                companyAddress
            };

            DataTable eft = Helper.CreateEFTTable(companyEFTDetail);
            DataTable address = Helper.CreateAddressesTable(addresses);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_AddCompanyDetail";
                    command.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = companyDetail.CompanyName;
                    command.Parameters.Add("@CompanyLogo", SqlDbType.VarBinary, companyDetail.CompanyLogo.Length).Value = companyDetail.CompanyLogo;
                    command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = companyDetail.PhoneNumber;
                    command.Parameters.Add("@EmailAddress", SqlDbType.NVarChar).Value = companyDetail.EmailAddress;
                    command.Parameters.Add("@Address", SqlDbType.Structured).Value = address;
                    command.Parameters.Add("@EFT", SqlDbType.Structured).Value = eft;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();
                            companyDetail.CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId"));
                            companyAddress.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                            companyEFTDetail.EFTId = reader.GetInt32(reader.GetOrdinal("EFTId"));
                        }
                    }
                }
            }
            return (companyDetail, companyEFTDetail, companyAddress);
        }

        public async Task<(CompanyDetailModel? CompanyDetailModel, CompanyEFTDetailModel? CompanyEFTDetailModel, AddressModel? CompanyAddressModel)> GetCompanyDetail()
        {
            CompanyDetailModel? companyDetailModel = new CompanyDetailModel();
            CompanyEFTDetailModel? companyEFTDetailModel = new CompanyEFTDetailModel();
            AddressModel? companyAddressModel = new AddressModel();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetCompanyDetail";

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            //read in company detail
                            await reader.ReadAsync();
                            companyDetailModel.CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId"));
                            companyDetailModel.CompanyName = reader.GetString(reader.GetOrdinal("CompanyName"));
                            companyDetailModel.CompanyLogo = (byte[])reader["CompanyLogo"];
                            companyDetailModel.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                            companyDetailModel.EFTId = reader.GetInt32(reader.GetOrdinal("EFTId"));
                            companyDetailModel.PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"));
                            companyDetailModel.EmailAddress = reader.GetString(reader.GetOrdinal("EmailAddress"));

                            await reader.NextResultAsync();
                            //read in company eft detail
                            await reader.ReadAsync();
                            companyAddressModel.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                            companyAddressModel.AddressLine1 = reader.GetString(reader.GetOrdinal("AddressLine1"));
                            if (!reader.IsDBNull(reader.GetOrdinal("AddressLine2")))
                            {
                                companyAddressModel.AddressLine2 = reader.GetString(reader.GetOrdinal("AddressLine2"));
                            }
                            companyAddressModel.Suburb = reader.GetString(reader.GetOrdinal("Suburb"));
                            companyAddressModel.City = reader.GetString(reader.GetOrdinal("City"));
                            companyAddressModel.PostalCode = reader.GetString(reader.GetOrdinal("PostalCode"));
                            companyAddressModel.Country = reader.GetString(reader.GetOrdinal("Country"));

                            await reader.NextResultAsync();
                            //read in company address detail
                            await reader.ReadAsync();
                            companyEFTDetailModel.EFTId = reader.GetInt32(reader.GetOrdinal("EFTId"));
                            companyEFTDetailModel.Bank = reader.GetString(reader.GetOrdinal("Bank"));
                            companyEFTDetailModel.AccountType = reader.GetString(reader.GetOrdinal("AccountType"));
                            companyEFTDetailModel.AccountNumber = reader.GetString(reader.GetOrdinal("AccountNumber"));
                            companyEFTDetailModel.BranchCode = reader.GetString(reader.GetOrdinal("BranchCode"));
                        }
                    }
                }
            }
            return (companyDetailModel, companyEFTDetailModel, companyAddressModel);
        }

    }
}

