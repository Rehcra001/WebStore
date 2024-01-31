using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebStore.API.Extentions;
using WebStore.API.Services.Contracts;
using WebStore.API.ValidationClasses;
using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        [Authorize]
        [Route("AddCartItem")]
        public async Task<ActionResult<CartItemDTO>> AddCartItem(CartItemDTO cartItemDTO)
        {
            try
            {
                //Convert to model
                CartItemModel cartItemModel = cartItemDTO.ConvertToCartItemDTO();
                //Validate model
                var cartItemErrors = ValidationHelper.Validate(cartItemModel);
                if (cartItemErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, cartItemErrors);
                }

                //Get user email address
                var userIdentity = User.Identity as ClaimsIdentity;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                cartItemModel = await _shoppingCartService.AddCartItem(cartItemModel, email);

                if (cartItemModel.CartItemId == 0)
                {
                    return NoContent();
                }

                return Ok(cartItemModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
