﻿using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        [Route("UpdateOrderShippedConfirmation/{orderId:int}")]
        public async Task<IActionResult> UpdateOrderShippedConfirmation(int orderId, [FromBody] ShippingConfirmationDTO shippingConfirmation)
        {
            try
            {
                await _orderServices.UpdateOrderShipped(orderId, shippingConfirmation.Shipped);

                OrderModel orderModel = await _orderServices.GetOrderById(orderId);
                OrderDTO orderDTO = orderModel.ConvertToOrderDTO();
                if (await _orderServices.SendShippingConfirmationEmail(orderDTO) == false)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error sending shipping order confirmation email");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating shipping confirmation");
            }
        }
    }
}
