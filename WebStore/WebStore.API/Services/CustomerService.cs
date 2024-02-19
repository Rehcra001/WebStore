using WebStore.API.Extentions;
using WebStore.API.Services.Contracts;
using WebStore.API.ValidationClasses;
using WebStore.DTO;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;
        private readonly ICompanyRepository _companyRepository;

        public CustomerService(ICustomerRepository customerRepository,
                               IEmailService emailService,
                               ICompanyRepository companyRepository)
        {
            _customerRepository = customerRepository;
            _emailService = emailService;
            _companyRepository = companyRepository;
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

        public async Task<AddressModel> AddCustomerAddress(AddressModel address, string email)
        {
            try
            {
                return await _customerRepository.AddAddress(address, email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<OrderModel> AddOrder(int addressId, string email)
        {
            try
            {
                return await _customerRepository.AddOrder(addressId, email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AddressModel> GetAddressById(int id, string email)
        {
            try
            {
                return await _customerRepository.GetAddressById(id, email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<AddressLineModel>> GetAddressLines(string email)
        {
            try
            {
                return await _customerRepository.GetAddressLines(email);
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

        public async Task<bool> SendOrderConfirmation(OrderDTO orderDTO, string emailTo)
        {
            try
            {
                var companyDetail = await _companyRepository.GetCompanyDetail();
                CompanyDetailDTO? companyDetailDTO = companyDetail.CompanyDetailModel!.ConvertToCompanyDetailDTO(companyDetail.CompanyEFTDetailModel!, companyDetail.CompanyAddressModel!);
                EmailDTO emailDto = orderDTO.ConvertToEmailBody(companyDetailDTO);
                emailDto.To = emailTo;
                emailDto.Subject = $"Order Confirmation for order# {orderDTO.OrderId}";

                await _emailService.SendEmailAsync(emailDto);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<CustomerModel> UpdateCustomer(CustomerModel customer)
        {
            throw new NotImplementedException();
        }
    }
}
