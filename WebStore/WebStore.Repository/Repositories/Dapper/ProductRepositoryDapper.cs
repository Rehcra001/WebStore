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

        public async Task<ProductModel> AddProduct(ProductModel product)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Name", product.Name);
            parameters.Add("@Description", product.Description);
            parameters.Add("@Picture", product.Picture, dbType: DbType.Binary, size: product.Picture.Length);
            parameters.Add("@Price", product.Price);
            parameters.Add("@QtyInStock", product.QtyInStock);
            parameters.Add("@UnitPerId", product.UnitPerId);
            parameters.Add("@CategoryId", product.CategoryId);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                try
                {
                    var returned = await connection.QuerySingleAsync<ProductModel>("dbo.usp_AddProduct", parameters, commandType: CommandType.StoredProcedure);
                    product.ProductId = returned.ProductId;
                }
                catch (Exception ex)
                {
                    product = default(ProductModel);
                }
            }
            return product;
        }

        public async Task<ProductCategoryModel> AddProductCategory(ProductCategoryModel productCategory)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CategoryName", productCategory.CategoryName, DbType.String, ParameterDirection.Input);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                try
                {
                    var returned = await connection.QuerySingleAsync<ProductCategoryModel>("dbo.usp_AddProductCategory", parameters, commandType: CommandType.StoredProcedure);
                    productCategory.ProductCategoryId = returned.ProductCategoryId;
                }
                catch (Exception)
                {
                    productCategory = default(ProductCategoryModel);
                }
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

        public async Task<IEnumerable<ProductCategoryModel>> GetAllCatergories()
        {
            List<ProductCategoryModel> productCategories = new List<ProductCategoryModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                productCategories = (List<ProductCategoryModel>)await connection.QueryAsync<ProductCategoryModel>("dbo.usp_GetAllCategories", commandType: CommandType.StoredProcedure);
            }

            return productCategories;
        }

        public async Task<IEnumerable<UnitPerModel>> GetAllUnitPers()
        {
            List<UnitPerModel> unitPers = new List<UnitPerModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                unitPers = (List<UnitPerModel>)await connection.QueryAsync<UnitPerModel>("dbo.usp_GetAllUnitPers", commandType: CommandType.StoredProcedure);
            }

            return unitPers;
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            ProductModel product = new ProductModel();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ProductId", id);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                try
                {
                    product = await connection.QuerySingleAsync<ProductModel>("dbo.usp_GetProduct", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    product = default(ProductModel);
                }
            }

            return product;
        }

        public async Task<ProductCategoryModel> GetProductCategory(int id)
        {
            ProductCategoryModel productCategory = new ProductCategoryModel();
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

        public async Task<IEnumerable<ProductModel>> GetProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                products = (List<ProductModel>)await connection.QueryAsync<ProductModel>("dbo.usp_GetProducts", commandType: CommandType.StoredProcedure);
            }

            return products;
        }

        public async Task<IEnumerable<ProductListModel>> GetProductsList()
        {
            List<ProductListModel> productsList = new List<ProductListModel>();

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                productsList = (List<ProductListModel>)await connection.QueryAsync<ProductListModel>("dbo.usp_GetProductsList", commandType: CommandType.StoredProcedure);
            }

            return productsList;
        }

        public async Task<UnitPerModel> GetUnitPer(int id)
        {
            UnitPerModel unitPer = new UnitPerModel();
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

        public async Task<ProductModel> UpdateProduct(ProductModel product)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ProductId", product.ProductId);
            parameters.Add("@Name", product.Name);
            parameters.Add("@Description", product.Description);
            parameters.Add("@Picture", product.Picture, dbType: DbType.Binary, size: product.Picture.Length);
            parameters.Add("@Price", product.Price);
            parameters.Add("@QtyInStock", product.QtyInStock);
            parameters.Add("@UnitPerId", product.UnitPerId);
            parameters.Add("@CategoryId", product.CategoryId);

            using (SqlConnection connection = _sqlConnection.SqlConnection())
            {
                ProductModel returned = await connection.QuerySingleAsync<ProductModel>("dbo.usp_UpdateProduct", parameters, commandType: CommandType.StoredProcedure);
                if (returned.ProductId == 0)
                {
                    //Return a new model if nothing returned as this means that the product was not saved
                    product = new ProductModel();
                }
            }
            return product;
        }
    }
}
