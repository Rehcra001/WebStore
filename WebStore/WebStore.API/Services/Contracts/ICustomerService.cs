using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerModel> AddCustomer(CustomerModel customer);
        Task<AddressModel> AddCustomerAddress(AddressModel address, string email);
        Task<OrderModel> AddOrder(int addressId, string email);        
        Task<CustomerModel> GetCustomer(string email);
        Task<IEnumerable<CustomerModel>> GetCustomers();
        Task<IEnumerable<AddressLineModel>> GetAddressLines(string email);
        Task<AddressModel> GetAddressById(int id, string email);
        Task<bool> SendOrderConfirmation(OrderDTO orderDTO, string emailTo);
        Task<bool> UpdateCustomerDetail(CustomerModel customer);
        Task<bool> UpdateCustomerAddress(AddressModel address);
    }
}
