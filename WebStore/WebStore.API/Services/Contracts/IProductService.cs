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
        Task<ProductCategoryModel> GetProductCategory(int id);
        Task<UnitPerModel> GetUnitPer(int id);
    }
}
