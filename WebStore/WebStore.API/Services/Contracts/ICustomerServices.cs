using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface ICustomerServices
    {
        Task<CustomerModel> AddCustomer(CustomerModel customer);
        Task<CustomerModel> UpdateCustomer(CustomerModel customer);
        Task<CustomerModel> GetCustomer(int id);
        Task<IEnumerable<CustomerModel>> GetCustomers();
    }
}
