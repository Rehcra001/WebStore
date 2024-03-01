using WebStore.Models;

namespace WebStore.Repository.Contracts
{
    public interface ICustomerRepository
    {
        Task<CustomerModel> AddCustomer(CustomerModel customer);
        Task<AddressModel> AddAddress(AddressModel address, string email);
        Task<OrderModel> AddOrder(int addressId, string email);
        Task<CustomerModel> GetCustomer(string email);
        Task<IEnumerable<OrderModel>> GetCustomerOrders(string email);
        Task<IEnumerable<AddressLineModel>> GetAddressLines(string email);
        Task<AddressModel> GetAddressById(int addressId, string email);
        Task<bool> UpdateCustomerAddress(AddressModel address);
        Task<bool> UpdateCustomerDetail(CustomerModel customer);
    }
}
