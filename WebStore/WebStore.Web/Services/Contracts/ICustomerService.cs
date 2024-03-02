using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface ICustomerService
    {
        Task<OrderDTO> AddOrderAsync(int addressId);
        Task<CustomerDTO> GetCustomerDetailsAsync();
        Task<IEnumerable<AddressLineDTO>> GetAddresLinesAsync();
        Task<AddressDTO> AddCustomerAddressAsync(AddressDTO addressDTO);
        Task<AddressDTO> GetAddressByIdAsync(int addressId);
        Task<IEnumerable<OrderDTO>> GetCustomerOrdersAsync();
        Task UpdateCustomerDetailAsync(UpdateCustomerDetailDTO updateCustomerDetailDTO);
        Task UpdateCustomerAddressAsync(AddressDTO addressDTO);
    }
}
