using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Task<UnitPerModel> AddUnitPer(UnitPerModel unitPer)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductCategoryModel> GetProductCategory(int id)
        {
            ProductCategoryModel productCategory = new ProductCategoryModel();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ProductCategoryId", id, DbType.Int32, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                productCategory = await connection.QuerySingleAsync<ProductCategoryModel>("dbo.usp_GetProductCategory", parameters, commandType: CommandType.StoredProcedure);
            }

            return productCategory;
        }
    }
}
