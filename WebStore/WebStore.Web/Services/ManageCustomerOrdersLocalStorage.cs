using Blazored.LocalStorage;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class ManageCustomerOrdersLocalStorage : IManageCustomerOrdersLocalStorage
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly ICustomerService _customerService;
        private const string KEY = "CustomerOrders";

        public ManageCustomerOrdersLocalStorage(ILocalStorageService localStorageService,
                                                ICustomerService customerService)
        {
            _localStorageService = localStorageService;
            _customerService = customerService;
        }

        public async Task<IEnumerable<OrderDTO>> GetCollection()
        {
            return await _localStorageService.GetItemAsync<IEnumerable<OrderDTO>>(KEY) ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorageService.RemoveItemAsync(KEY);
        }

        public async Task SaveCollection(List<OrderDTO> orderDTOs)
        {
            await _localStorageService.SetItemAsync(KEY, orderDTOs);
        }

        private async Task<IEnumerable<OrderDTO>> AddCollection()
        {
            var customerOrders = await _customerService.GetCustomerOrdersAsync();
            if (customerOrders != null)
            {
                await _localStorageService.SetItemAsync(KEY, customerOrders);
            }
            return customerOrders;
        }
    }
}
