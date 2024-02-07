using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerModel> AddCustomer(CustomerModel customer);
        Task<AddressModel> AddCustomerAddress(AddressModel address, string email);
        Task<CustomerModel> UpdateCustomer(CustomerModel customer);
        Task<CustomerModel> GetCustomer(string email);
        Task<IEnumerable<CustomerModel>> GetCustomers();
        Task<IEnumerable<AddressLineModel>> GetAddressLines(string email);
    }
}
