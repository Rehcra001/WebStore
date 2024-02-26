using WebStore.API.Extentions;
using WebStore.API.Services.Contracts;
using WebStore.DTO;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.API.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailService _emailService;

        public OrderServices(IOrderRepository orderRepository,
                             ICompanyRepository companyRepository,
                             IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _companyRepository = companyRepository;
            _emailService = emailService;
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

        public async Task<bool> SendShippingConfirmationEmail(OrderDTO orderDTO)
        {
            try
            {
                var companyDetail = await _companyRepository.GetCompanyDetail();
                CompanyDetailDTO? companyDetailDTO = companyDetail.CompanyDetailModel!.ConvertToCompanyDetailDTO(companyDetail.CompanyEFTDetailModel!,
                                                                                                                 companyDetail.CompanyAddressModel!);
                string message = "This email confirms that your order has shipped";
                EmailDTO emailDTO = orderDTO.ConvertToOrderEmailBody(companyDetailDTO, message);
                emailDTO.To = orderDTO.EmailAddress;
                emailDTO.Subject = $"Shipping confirmation for order#: {orderDTO.OrderId}";

                await _emailService.SendEmailAsync(emailDTO);

                return true;
            }
            catch (Exception)
            {
                return false;
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
