using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IManageProductCategoriesLocalStorageService
    {
        Task<IEnumerable<ProductCategoryDTO>> GetCollection();
        Task RemoveCollection();
    }
}
