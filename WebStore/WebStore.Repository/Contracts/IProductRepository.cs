using WebStore.Models;

namespace WebStore.Repository.Contracts
{
    public interface IProductRepository
    {
        Task<ProductModel> AddProduct(ProductModel product);
        Task<ProductCategoryModel> AddProductCategory(ProductCategoryModel productCategory);
        Task<UnitPerModel> AddUnitPer(UnitPerModel unitPer);
        Task<ProductModel> GetProduct(int id);
        Task<ProductCategoryModel> GetProductCategory(int id);
        Task<UnitPerModel> GetUnitPer(int id);
    }
}
