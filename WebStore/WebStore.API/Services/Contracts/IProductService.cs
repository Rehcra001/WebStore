using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface IProductService
    {
        Task<ProductModel> AddProduct(ProductModel product);
        Task<ProductCategoryModel> AddProductCategory(ProductCategoryModel productCategory);
        Task<UnitPerModel> AddUnitPer(UnitPerModel unitPer);
        Task<IEnumerable<ProductCategoryModel>> GetAllCatergories();
        Task<IEnumerable<UnitPerModel>> GetAllUnitPers();
        Task<ProductModel> GetProduct(int id);
        Task<IEnumerable<ProductModel>> GetProducts();
        Task<IEnumerable<ProductListModel>> GetProductsList();
        Task<ProductCategoryModel> GetProductCategory(int id);
        Task<UnitPerModel> GetUnitPer(int id);
        Task<ProductModel> UpdateProduct(ProductModel product);
        Task<ProductCategoryModel> UpdateProductCategory(ProductCategoryModel productCategory);
        Task<UnitPerModel> UpdateUnitPer(UnitPerModel unitPer);
        Task UpdateStockQuantities(OrderModel order);
    }
}
