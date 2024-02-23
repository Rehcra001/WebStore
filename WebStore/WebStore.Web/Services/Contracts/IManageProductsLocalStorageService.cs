using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IManageProductsLocalStorageService
    {
        Task<IEnumerable<ProductDTO>> GetCollection();
        Task RemoveCollection();
        Task SaveCollection(List<ProductDTO> products);
    }
}
