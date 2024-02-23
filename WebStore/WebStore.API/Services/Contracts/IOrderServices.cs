using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface IOrderServices
    {
        Task<IEnumerable<OrderModel>> GetOrdersToBeShipped();
        Task<IEnumerable<OrderModel>> GetOrdersWithOutstandingPayment();
        Task UpdateOrderPayment(int orderId, bool payed);
        Task UpdateOrderShipped(int orderId, bool shipped);
    }
}
