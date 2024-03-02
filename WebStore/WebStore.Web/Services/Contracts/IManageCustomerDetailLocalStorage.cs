using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IManageCustomerDetailLocalStorage
    {
        Task<CustomerDTO> GetCustomerDetail();
        Task RemoveCustomerDetail();
        Task SaveCustomerDetail(CustomerDTO customerDTO);
    }
}
