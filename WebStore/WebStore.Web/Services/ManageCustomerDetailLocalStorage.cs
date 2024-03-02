using Blazored.LocalStorage;
using System.Security.AccessControl;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class ManageCustomerDetailLocalStorage : IManageCustomerDetailLocalStorage
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly ICustomerService _customerService;
        private const string KEY = "CustomerDetail";


        public ManageCustomerDetailLocalStorage(ILocalStorageService localStorageService,
                                                ICustomerService customerService)
        {
            _localStorageService = localStorageService;
            _customerService = customerService;
        }

        public async Task<CustomerDTO> GetCustomerDetail()
        {
            return await _localStorageService.GetItemAsync<CustomerDTO>(KEY) ?? await AddCustomerDetail();
        }

        public async Task RemoveCustomerDetail()
        {
            await _localStorageService.RemoveItemAsync(KEY);
        }

        public async Task SaveCustomerDetail(CustomerDTO customerDTO)
        {
            await _localStorageService.SetItemAsync(KEY, customerDTO);
        }

        private async Task<CustomerDTO> AddCustomerDetail()
        {
            var customerDetail = await _customerService.GetCustomerDetailsAsync();
            if (customerDetail != null)
            {
                await _localStorageService.SetItemAsync(KEY, customerDetail);
            }
            return customerDetail;
        }
    }
}
