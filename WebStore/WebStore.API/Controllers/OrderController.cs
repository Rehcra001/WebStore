using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
        [Route("UpdateOrderPaymentConfirmation")]
        public async Task<IActionResult> UpdateOrderPaymentConfirmation(int orderId, bool payed)
        {
            try
            {
                await _orderServices.UpdateOrderPayment(orderId, payed);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating Payment confirmation");
            }
        }

        [HttpPatch]
        [Authorize]
        [Route("UpdateOrderShippedConfirmation")]
        public async Task<IActionResult> UpdateOrderShippedConfirmation(int orderId, bool shipped)
        {
            try
            {
                await _orderServices.UpdateOrderShipped(orderId, shipped);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating shipping confirmation");
            }
        }
    }
}
