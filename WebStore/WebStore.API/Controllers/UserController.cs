using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebStore.DTO;
using WebStore.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using WebStore.API.Extentions;
using WebStore.API.Services.Contracts;
using WebStore.API.ValidationClasses;

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ICustomerService _customerService;

        public UserController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IConfiguration config,
                              ICustomerService customerService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _customerService = customerService;
        }

        [Route("register")] // domain/api/user/register
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userDTO)
        {
            //convert to UserRegistrationModel
            UserRegistrationModel userRegistration = userDTO.ConvertToUserRegistrationModel();

            //Validate user registration
            var registrationErrors = ValidationHelper.Validate(userRegistration);
            if (registrationErrors.Count > 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, registrationErrors);
            }

            //convert to customer model
            CustomerModel customer = userDTO.ConvertToCustomerModel();

            //Validate customer model
            var customerErrors = ValidationHelper.Validate(customer);
            if (customerErrors.Count > 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, customerErrors);
            }

            //Validate address models
            foreach (AddressModel address in customer.AddressList)
            {
                var addressErrors = ValidationHelper.Validate(address);
                if (addressErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, addressErrors);
                }
            }

            string userName = userDTO.EmailAddress;
            string password = userDTO.Password;

            // create user
            IdentityUser identityUser = new IdentityUser
            {
                Email = userName,
                UserName = userName
            };

            // Register the user
            IdentityResult identityResult = await _userManager.CreateAsync(identityUser, password);

            // If registration successful
            if (identityResult.Succeeded == true)
            {
                try
                {
                    //Save the customer data
                    await _customerService.AddCustomer(customer);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error saving customer data to database");
                }

                return Ok(new { identityResult.Succeeded });
            }
            else
            {
                string errorsToReturn = "Registration failed with the following errors";

                foreach (var error in identityResult.Errors)
                {
                    errorsToReturn += Environment.NewLine;
                    errorsToReturn += $"Error Code: {error.Code} - {error.Description}";
                }

                //send the error
                return StatusCode(StatusCodes.Status500InternalServerError, errorsToReturn);
            }
        }
    }
}
