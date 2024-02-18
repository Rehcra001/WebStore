using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStore.Models;
using WebStore.Repository.Contracts;
using WebStore.Repository.Static;

namespace WebStore.Repository.Repositories.Dapper
{
    public class CompanyRepositoryDapper : ICompanyRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public CompanyRepositoryDapper(IRelationalDatabaseConnection sqlConnection)
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

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyName", companyDetail.CompanyName, DbType.String);
            parameters.Add("@CompanyLogo", companyDetail.CompanyLogo, DbType.Binary, size: companyDetail.CompanyLogo.Length);
            parameters.Add("@PhoneNumber", companyDetail.PhoneNumber, DbType.String);
            parameters.Add("@EmailAddress", companyDetail.EmailAddress, DbType.String);
            parameters.Add("@Address", address, DbType.Object);
            parameters.Add("@EFT", eft, DbType.Object);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                var returned = await connection.QuerySingleAsync<CompanyDetailIdsModel>("dbo.usp_AddCompanyDetail", parameters, commandType: CommandType.StoredProcedure);

                companyDetail.CompanyId = returned.CompanyId;
                companyEFTDetail.EFTId = returned.EFTId;
                companyAddress.AddressId = returned.AddressId;
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
                using (var multiResultSets = await connection.QueryMultipleAsync("dbo.usp_GetCompanyDetail", commandType: CommandType.StoredProcedure))
                {
                    companyDetailModel = await multiResultSets.ReadSingleOrDefaultAsync<CompanyDetailModel>();
                    companyAddressModel = await multiResultSets.ReadSingleOrDefaultAsync<AddressModel>();
                    companyEFTDetailModel = await multiResultSets.ReadSingleOrDefaultAsync<CompanyEFTDetailModel>();
                    
                }

            }
            return (companyDetailModel, companyEFTDetailModel, companyAddressModel);
        }
    }
}
