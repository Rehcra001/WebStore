using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Services.Contracts
{
    public interface IOrderServices
    {
        Task<OrderModel> GetOrderById(int id);
        Task<IEnumerable<OrderModel>> GetOrdersToBeShipped();
        Task<IEnumerable<OrderModel>> GetOrdersWithOutstandingPayment();
        Task<bool> SendShippingConfirmationEmail(OrderDTO orderDTO);
        Task UpdateOrderPayment(int orderId, bool payed);
        Task UpdateOrderShipped(int orderId, bool shipped);
    }
}
