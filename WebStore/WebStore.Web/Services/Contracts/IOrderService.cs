using WebStore.DTO;

namespace WebStore.WEB.Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersToBeShipped();
        Task<IEnumerable<OrderDTO>> GetOrdersWithOutstandingPayment();
        Task UpdateOrderPayment(int orderId, bool payed);
        Task UpdateOrderShipped(int orderId, bool shipped);
    }
}
