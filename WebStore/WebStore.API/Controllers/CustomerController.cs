using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebStore.API.Extentions;
using WebStore.API.Services.Contracts;
using WebStore.DTO;
using WebStore.Models;
using static Dapper.SqlMapper;

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
        public async Task<ActionResult<CustomerDTO>> GetCustomer()
        {
            try
            {
                var userIdentity = User.Identity as ClaimsIdentity ;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (String.IsNullOrWhiteSpace(email))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "CustomerController: No user name found");
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
    }
}
