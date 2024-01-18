using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.Repository.Repositories.Dapper
{
    public class ProductRepositoryDapper : IProductRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public ProductRepositoryDapper(IRelationalDatabaseConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public Task<ProductModel> AddProduct(ProductModel product)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductCategoryModel> AddProductCategory(ProductCategoryModel productCategory)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CategoryName", productCategory.CategoryName, DbType.String, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                productCategory = await connection.QuerySingleAsync<ProductCategoryModel>("dbo.usp_AddProductCategory", parameters, commandType: CommandType.StoredProcedure);
            }
            return productCategory;
        }

        public async Task<UnitPerModel> AddUnitPer(UnitPerModel unitPer)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UnitPer", unitPer.UnitPer, DbType.String, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                unitPer = await connection.QuerySingleAsync<UnitPerModel>("dbo.usp_AddUnitPer", parameters, commandType: CommandType.StoredProcedure);
            }
            return unitPer;
        }

        public Task<ProductModel> GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductCategoryModel> GetProductCategory(int id)
        {
            ProductCategoryModel? productCategory = new ProductCategoryModel();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ProductCategoryId", id, DbType.Int32, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                try
                {
                    productCategory = await connection.QuerySingleAsync<ProductCategoryModel>("dbo.usp_GetProductCategory", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    productCategory = default(ProductCategoryModel);
                }
            }

            return productCategory;
        }

        public async Task<UnitPerModel> GetUnitPer(int id)
        {
            UnitPerModel? unitPer = new UnitPerModel();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UnitPerId", id, DbType.Int32, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                try
                {
                    unitPer = await connection.QuerySingleAsync<UnitPerModel>("dbo.usp_GetUnitPer", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    //set unitPer to default
                    unitPer = default(UnitPerModel);
                } 
            }
            
            return unitPer;
        }
    }
}
