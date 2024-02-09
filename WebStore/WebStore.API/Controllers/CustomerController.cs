using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebStore.API.Extentions;
using WebStore.API.Services.Contracts;
using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize]
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

                if (customer.CustomerId == default || customer.AddressList == default)
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
        [Authorize]
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
        [Authorize]
        [Route("AddCustomerAddress")]
        public async Task<ActionResult<AddressDTO>> AddCustomerAddress(AddressDTO addressDTO)
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
        [Authorize]
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
    }
}
