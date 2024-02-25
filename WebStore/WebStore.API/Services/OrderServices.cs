using WebStore.API.Services.Contracts;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.API.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;

        public OrderServices(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<OrderModel> GetOrderById(int id)
        {
            try
            {
                return _orderRepository.GetOrderById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersToBeShipped()
        {
            try
            {
                return await _orderRepository.GetOrdersToBeShipped();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersWithOutstandingPayment()
        {
            try
            {
                return await _orderRepository.GetOrdersWithOutstandingPayment();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateOrderPayment(int orderId, bool payed)
        {
            try
            {
                await _orderRepository.UpdateOrderPayment(orderId, payed);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateOrderShipped(int orderId, bool shipped)
        {
            try
            {
                await _orderRepository.UpdateOrderShipped(orderId, shipped);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
