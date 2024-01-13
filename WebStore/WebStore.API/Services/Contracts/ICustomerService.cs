using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerModel> AddCustomer(CustomerModel customer);
        //Task<CustomerModel> AddCustomerAddress(AddressModel address);
        Task<CustomerModel> UpdateCustomer(CustomerModel customer);
        Task<CustomerModel> GetCustomer(string email);
        Task<IEnumerable<CustomerModel>> GetCustomers();
    }
}
