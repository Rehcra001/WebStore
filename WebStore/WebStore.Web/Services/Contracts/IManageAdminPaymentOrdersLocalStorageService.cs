using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IManageAdminPaymentOrdersLocalStorageService
    {
        Task<IEnumerable<OrderDTO>> GetCollection();
        Task RemoveCollection();
        Task SaveCollection(List<OrderDTO> orderDTOs);
    }
}
