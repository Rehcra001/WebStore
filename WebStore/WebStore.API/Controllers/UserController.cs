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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly ICustomerService _customerService;

        public UserController(SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IConfiguration config,
                              ICustomerService customerService,
                              RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _customerService = customerService;
            _roleManager = roleManager;
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
            IdentityResult roleIdentityResult = await _userManager.AddToRoleAsync(identityUser, "Customer");
            // If registration successful
            if (identityResult.Succeeded == true && roleIdentityResult.Succeeded)
            {
                
                try
                {
                    //Save the customer data
                    await _customerService.AddCustomer(customer);
                }
                catch (Exception)
                {
                    // delete the user added to the login db
                    await _userManager.DeleteAsync(identityUser);

                    return StatusCode(StatusCodes.Status500InternalServerError, "Error saving customer data to database");
                }

                return Ok(new { identityResult.Succeeded });
            }
            else
            {
                string errorsToReturn = "Registration failed with the following errors: ";

                foreach (var error in identityResult.Errors)
                {
                    errorsToReturn += "\\r\\n";
                    errorsToReturn += $"Error Code: {error.Code} - {error.Description}";
                }

                //send the error
                return StatusCode(StatusCodes.Status500InternalServerError, errorsToReturn);
            }
        }

        [Route("signin")] //domain/api/user/signin
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] UserSignInDTO userSignInDTO)
        {
            //Convert to model and validate
            UserSignInModel userSignInModel = userSignInDTO.ConvertToUserSignInModel();

            //Validate
            var signinErrors = ValidationHelper.Validate(userSignInModel);
            if (signinErrors.Count > 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, signinErrors);
            }


            string userName = userSignInModel.EmailAddress;
            string password = userSignInModel.Password;

            //Try to sign the user in
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            // If the sign in was successful
            if (signInResult.Succeeded == true)
            {
                IdentityUser identityUser = await _userManager.FindByNameAsync(userName);

                string JSONWebTokenAsString = await GenerateJSONWebToken(identityUser);

                return Ok(JSONWebTokenAsString);
            }
            else
            {
                return Unauthorized(userSignInDTO);
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<string> GenerateJSONWebToken(IdentityUser? identityUser)
        {
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            //claim = whoe is the person trying to sign in claiming to be
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, identityUser.Id)
            };

            IList<string> roleNames = await _userManager.GetRolesAsync(identityUser);
            claims.AddRange(roleNames.Select(roleName => new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)));

            //Generate the token
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                    _config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    null,
                    expires: DateTime.UtcNow.AddDays(28),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

    }
}
