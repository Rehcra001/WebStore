using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface IProductService
    {
        Task<ProductModel> AddProduct(ProductModel product);
        Task<ProductCategoryModel> AddProductCategory(ProductCategoryModel productCategory);
        Task<UnitPerModel> AddUnitPer(UnitPerModel unitPer);
        Task<ProductCategoryModel> GetProductCategory(int id);
    }
}
