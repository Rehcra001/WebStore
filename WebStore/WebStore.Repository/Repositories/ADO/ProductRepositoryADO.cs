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

        public async Task<ProductModel> AddProduct(ProductModel product)
        {
            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_AddProduct";
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = product.Name;
                    command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = product.Description;
                    command.Parameters.AddWithValue("@Picture", product.Picture);
                    command.Parameters.Add("@Price", SqlDbType.Money).Value = product.Price;
                    command.Parameters.Add("@QtyInStock", SqlDbType.Int).Value = product.QtyInStock;
                    command.Parameters.Add("@UnitPerId", SqlDbType.Int).Value = product.UnitPerId;
                    command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = product.CategoryId;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();

                            product.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"));
                        }
                    }
                }
            }
            return product;
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

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
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

        public async Task<UnitPerModel> AddUnitPer(UnitPerModel unitPer)
        {
            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_AddUnitPer";
                    command.Parameters.Add("@UnitPer", SqlDbType.NVarChar).Value = unitPer.UnitPer;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();
                            unitPer.UnitPerId = reader.GetInt32(reader.GetOrdinal("UnitPerId"));
                        }
                    }
                }
            }
            return unitPer;
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            ProductModel product = new ProductModel();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetProduct";
                    command.Parameters.Add("@ProductId", SqlDbType.Int).Value = id;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader =  await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();
                            product.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"));
                            product.Name = reader.GetString(reader.GetOrdinal("Name"));
                            product.Description = reader.GetString(reader.GetOrdinal("Description"));
                            product.Picture = (byte[])reader["Picture"];
                            product.Price = reader.GetDecimal(reader.GetOrdinal("Price"));
                            product.QtyInStock = reader.GetInt32(reader.GetOrdinal("QtyInStock"));
                            product.UnitPerId = reader.GetInt32(reader.GetOrdinal("UnitPerId"));
                            product.CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));
                            product.UnitPer = reader.GetString(reader.GetOrdinal("UnitPer"));
                            product.CategoryName = reader.GetString(reader.GetOrdinal("CategoryName"));                        }
                    }
                }
            }
            return product;
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

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
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

        public async Task<UnitPerModel> GetUnitPer(int id)
        {
            UnitPerModel unitPer = new UnitPerModel();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "dbo.usp_GetUnitPer";
                    command.Parameters.Add("@UnitPerId", SqlDbType.Int).Value = id;

                    await command.Connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();

                            unitPer.UnitPerId = reader.GetInt32(reader.GetOrdinal("UnitPerId"));
                            unitPer.UnitPer = reader.GetString(reader.GetOrdinal("UnitPer"));
                        }
                    }
                }
            }
            return unitPer;
        }
    }
}
