using Blazored.LocalStorage;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Services
{
    public class ManageAdminPaymentOrdersLocalStorageService : IManageAdminPaymentOrdersLocalStorageService
    {
        private readonly IOrderService _orderService;
        private readonly ILocalStorageService _localStorageService;
        private const string KEY = "PaymentConfirmationOrderCollection";

        public ManageAdminPaymentOrdersLocalStorageService(IOrderService orderService,
                                                           ILocalStorageService localStorageService)
        {
            _orderService = orderService;
            _localStorageService = localStorageService;
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
            IEnumerable<OrderDTO> orderDTOs = await _orderService.GetOrdersWithOutstandingPayment();
            if (orderDTOs != null)
            {
                await _localStorageService.SetItemAsync(KEY, orderDTOs);
            }
            return orderDTOs;
        }
    }
}
