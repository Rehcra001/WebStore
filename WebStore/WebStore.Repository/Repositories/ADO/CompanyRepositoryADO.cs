using Microsoft.Data.SqlClient;
using System.Data;
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

        public async Task<(CompanyDetailModel companyDetailModel, CompanyEFTDetailModel companyEFTDetailModel, AddressModel companyAddressModel)> AddCompanyDetail(CompanyDetailModel companyDetail, CompanyEFTDetailModel companyEFTDetail, AddressModel companyAddress)
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
                            companyDetail.CompanyId = reader.GetInt32(reader.GetOrdinal("CompanyId"));
                            companyAddress.AddressId = reader.GetInt32(reader.GetOrdinal("AddressId"));
                            companyEFTDetail.EFTId = reader.GetInt32(reader.GetOrdinal("EFTId"));
                        }
                    }
                }
            }
            return (companyDetail, companyEFTDetail, companyAddress);
        }
    }
}
