﻿using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerDTO> GetCustomerDetailsAsync();
        Task<IEnumerable<AddressLineDTO>> GetAddresLinesAsync();
        Task<AddressDTO> AddCustomerAddressAsync(AddressDTO addressDTO);
        Task<AddressDTO> GetAddressByIdAsync(int addressId);
    }
}
