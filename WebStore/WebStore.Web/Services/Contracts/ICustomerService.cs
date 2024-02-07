using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerDTO> GetCustomerDetailsAsync();
        Task<IEnumerable<AddressLineDTO>> GetAddresLinesAsync();
        Task<AddressDTO> AddCustomerAddress(AddressDTO addressDTO);
    }
}
