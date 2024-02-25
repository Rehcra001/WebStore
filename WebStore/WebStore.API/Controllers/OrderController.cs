using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.API.Extentions;
using WebStore.API.Services.Contracts;
using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet]
        [Authorize]
        [Route("GetOrderById/{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            try
            {
                OrderModel orderModel = await _orderServices.GetOrderById(id);
                if (orderModel == null || orderModel.OrderId == 0)
                {
                    return NoContent();
                }

                OrderDTO orderDTO = orderModel.ConvertToOrderDTO();
                return Ok(orderDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to retrieve the order");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetOrdersToBeShipped")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersToBeShipped()
        {
            try
            {
                IEnumerable<OrderModel> orderModels = await _orderServices.GetOrdersToBeShipped();
                if (orderModels == null || !orderModels.Any())
                {
                    return NoContent();
                }
                else
                {
                    //Convert to DTO
                    IEnumerable<OrderDTO> orderDTOs = orderModels.ConvertToOrderDTOs();

                    return Ok(orderDTOs);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving orders that need to be shipped");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetOrdersWithOutstandingPayment")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersWithOutstandingPayment()
        {
            try
            {
                IEnumerable<OrderModel> orderModels = await _orderServices.GetOrdersWithOutstandingPayment();

                if (orderModels == null || !orderModels.Any())
                {
                    return NoContent();
                }
                else
                {
                    //Convert to DTO
                    IEnumerable<OrderDTO> orderDTOs = orderModels.ConvertToOrderDTOs();

                    return Ok(orderDTOs);
                }
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving orders with outstanding payments");
            }
        }

        [HttpPatch]
        [Authorize]
        [Route("UpdateOrderPaymentConfirmation/{orderId:int}")]
        public async Task<IActionResult> UpdateOrderPaymentConfirmation(int orderId, [FromBody] PaymentConfirmationDTO paymentConfirmation)
        {
            try
            {
                await _orderServices.UpdateOrderPayment(orderId, paymentConfirmation.Payed);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating Payment confirmation");
            }
        }

        [HttpPatch]
        [Authorize]
        [Route("UpdateOrderShippedConfirmation/{orderId:int}")]
        public async Task<IActionResult> UpdateOrderShippedConfirmation(int orderId, [FromBody] ShippingConfirmationDTO shippingConfirmation)
        {
            try
            {
                await _orderServices.UpdateOrderShipped(orderId, shippingConfirmation.Shipped);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating shipping confirmation");
            }
        }
    }
}
