using WebStore.Models;

namespace WebStore.Repository.Contracts
{
    public interface IOrderRepository
    {
        Task<OrderModel> GetOrderById(int id);
        Task<IEnumerable<OrderModel>> GetOrdersToBeShipped();
        Task<IEnumerable<OrderModel>> GetOrdersWithOutstandingPayment();
        Task UpdateOrderPayment(int orderId, bool payed);
        Task UpdateOrderShipped(int orderId, bool shipped);
    }
}
