using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.Repository.Repositories.ADO
{
    public class ProductRepositoryADO : IProductRepository
    {
        private readonly IRelationalDatabaseConnection _sqlConnection;

        public ProductRepositoryADO(IRelationalDatabaseConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public Task<ProductModel> AddProduct(ProductModel product)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductCategoryModel> AddProductCategory(ProductCategoryModel productCategory)
        {
            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_AddProductCategory";
                    command.Parameters.Add("@CategoryName", SqlDbType.NVarChar).Value = productCategory.CategoryName;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();
                            productCategory.ProductCategoryId = reader.GetInt32(reader.GetOrdinal("ProductCategoryId"));
                        }
                    }
                }
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

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "usp_GetProductCategory";
                    command.Parameters.Add("@ProductCategoryId", SqlDbType.Int).Value = id;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();
                            productCategory.ProductCategoryId = reader.GetInt32(reader.GetOrdinal("ProductCategoryId"));
                            productCategory.CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"));
                        }
                    }
                }
            }
            return productCategory;
        }
    }
}
