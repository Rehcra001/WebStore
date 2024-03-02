using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebStore.API.Extentions;
using WebStore.API.Services;
using WebStore.API.Services.Contracts;
using WebStore.API.ValidationClasses;
using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        public CustomerController(ICustomerService customerService, IProductService productService)
        {
            _customerService = customerService;
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("GetCustomer")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer()
        {
            try
            {
                var userIdentity = User.Identity as ClaimsIdentity ;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (String.IsNullOrWhiteSpace(email))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                CustomerModel customer = await _customerService.GetCustomer(email);

                if (customer.CustomerId == default || customer.AddressList == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving customer from the database");
                }
                else
                {
                    CustomerDTO customerDTO = customer.ConvertToCustomerDTO();
                    return Ok(customerDTO);
                }


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving customer from the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("GetCustomerOrders")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetCustomerOrders()
        {
            try
            {
                var userIdentity = User.Identity as ClaimsIdentity;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (String.IsNullOrWhiteSpace(email))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                IEnumerable<OrderModel> orderModels = await _customerService.GetCustomerOrders(email);
                if (orderModels == null || orderModels.Count() == 0)
                {
                    return NoContent();
                }
                else
                {
                    //convert to dto
                    IEnumerable<OrderDTO> orderDTOs = orderModels.ConvertToOrderDTOs();

                    return Ok(orderDTOs);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving customer from the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("GetAddressLines")]
        public async Task<ActionResult<IEnumerable<AddressLineDTO>>> GetAddressLines()
        {
            try
            {
                var userIdentity = User.Identity as ClaimsIdentity;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (String.IsNullOrWhiteSpace(email))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                IEnumerable<AddressLineModel> addressLineModels = await _customerService.GetAddressLines(email);

                if (addressLineModels == null || addressLineModels.Count() == 0)
                {
                    return NoContent();
                }
                else
                {
                    //convert
                    IEnumerable<AddressLineDTO> addressLineDTOs = addressLineModels.ConvertToAddressLineDTO();

                    return Ok(addressLineDTOs);
                    
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving customer from the database");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [Route("AddCustomerAddress")]
        public async Task<ActionResult<AddressDTO>> AddCustomerAddress([FromBody] AddressDTO addressDTO)
        {
            try
            {
                var userIdentity = User.Identity as ClaimsIdentity;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (String.IsNullOrWhiteSpace(email))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                //convert to model
                AddressModel addressModel = addressDTO.ConvertToAddressModel();

                //Validate address

                //save
                addressModel = await _customerService.AddCustomerAddress(addressModel, email);
                if (addressModel == null || addressModel.AddressId == 0)
                {
                    return NoContent();
                }
                else
                {
                    //convert to DTO
                    addressDTO = addressModel.ConvertToAddressDTO();
                    return Ok(addressDTO);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving address to the database");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        [Route("GetAddress/{id:int}")]
        public async Task<ActionResult<AddressDTO>> GetAddress(int id)
        {
            try
            {
                var userIdentity = User.Identity as ClaimsIdentity;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (String.IsNullOrWhiteSpace(email))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                AddressModel addressModel = await _customerService.GetAddressById(id, email);
                if (addressModel == null || addressModel.AddressId == default)
                {
                    return NoContent();
                }
                else
                {
                    //convert to DTO
                    AddressDTO addressDTO = addressModel.ConvertToAddressDTO();
                    return Ok(addressDTO);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving address from the database");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [Route("AddOrder/{addressId:int}")]
        public async Task<ActionResult<OrderDTO>> AddOrder(int addressId)
        {
            try
            {
                var userIdentity = User.Identity as ClaimsIdentity;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (String.IsNullOrWhiteSpace(email))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                OrderModel orderModel = await _customerService.AddOrder(addressId, email);

                if (orderModel == null || orderModel.OrderId == default)
                {
                    return NoContent();
                }

                //Convert to dto
                OrderDTO orderDTO = orderModel.ConvertToOrderDTO();

                //Send order confirmation email
                if (await _customerService.SendOrderConfirmation(orderDTO, email) == false)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error sending order confirmation email");
                }

                return Ok(orderDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving order from the database");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Customer")]
        [Route("UpdateCustomerDetail")]
        public async Task<IActionResult> UpdateCustomerDetail([FromBody] UpdateCustomerDetailDTO updateCustomerDetailDTO)
        {
            try
            {
                //Convert to model
                CustomerModel customerModel = updateCustomerDetailDTO.ConvertToCustomerDetailModel();

                //Validate model
                var customerDetailErrors = ValidationHelper.Validate(customerModel);
                if (customerDetailErrors.Count > 0)
                {
                    return BadRequest(customerDetailErrors);
                }

                // Update details
                bool updated = await _customerService.UpdateCustomerDetail(customerModel);

                if (updated == false)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error updating customer detail");
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating customer detail");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Customer")]
        [Route("UpdateCustomerAddress")]
        public async Task<IActionResult> UpdateCustomerAddress(AddressDTO addressDTO)
        {
            try
            {
                // Convert to model
                AddressModel addressModel = addressDTO.ConvertToAddressModel();

                // Validate
                var addressErrors = ValidationHelper.Validate(addressModel);
                if (addressErrors.Count > 0)
                {
                    return BadRequest(addressErrors);
                }

                // update address
                bool updated = await _customerService.UpdateCustomerAddress(addressModel);
                if (updated == false)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error updating address");
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating address");
            }
        }
    }
}
