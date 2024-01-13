using WebStore.Models;

namespace WebStore.Repository.Contracts
{
    public interface ICustomerRepository
    {
        Task<CustomerModel> AddCustomer(CustomerModel customer);
        Task<CustomerModel> UpdateCustomer(CustomerModel customer);
        Task<CustomerModel> GetCustomer(string email);
        Task<IEnumerable<CustomerModel>> GetCustomers();
    }
}
