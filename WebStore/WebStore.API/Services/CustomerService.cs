using WebStore.API.Services.Contracts;
using WebStore.API.ValidationClasses;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerModel> AddCustomer(CustomerModel customer)
        {
            try
            {
                return await _customerRepository.AddCustomer(customer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CustomerModel> GetCustomer(string email)
        {
            try
            {
                return await _customerRepository.GetCustomer(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<IEnumerable<CustomerModel>> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public Task<CustomerModel> UpdateCustomer(CustomerModel customer)
        {
            throw new NotImplementedException();
        }
    }
}
