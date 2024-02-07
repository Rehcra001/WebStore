using WebStore.Models;

namespace WebStore.Repository.Contracts
{
    public interface ICustomerRepository
    {
        Task<CustomerModel> AddCustomer(CustomerModel customer);
        Task<AddressModel> AddAddress(AddressModel address, string email);
        Task<CustomerModel> UpdateCustomer(CustomerModel customer);
        Task<CustomerModel> GetCustomer(string email);
        Task<IEnumerable<CustomerModel>> GetCustomers();
        Task<IEnumerable<AddressLineModel>> GetAddressLines(string email);
    }
}
