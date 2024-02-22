using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IManageCartItemsLocalStorageService
    {
        Task<IEnumerable<CartItemDTO>> GetCollection();
        Task RemoveCollection();
        Task SaveCollection(List<CartItemDTO> cartItemDTOs);
    }
}
