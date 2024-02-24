using Microsoft.AspNetCore.Authorization;
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
        private readonly IProductService _productService;

        public ShoppingCartController(IShoppingCartService shoppingCartService,
                                      IProductService productService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
        }

        [HttpPost]
        [Authorize]
        [Route("AddCartItem")]
        public async Task<ActionResult<CartItemDTO>> AddCartItem([FromBody] CartItemAddToDTO cartItemAddToDTO)
        {
            try
            {
                //Convert to model
                CartItemModel cartItemModel = cartItemAddToDTO.ConvertToCartItemModel();
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

                //get product
                ProductModel productModel = await _productService.GetProduct(cartItemModel.ProductId);
                if (productModel.ProductId == default)
                {
                    return NoContent();
                }

                //ConvertTo CartItemDTO
                CartItemDTO cartItemDTO = cartItemModel.ConvertToCartItemDTO(productModel);
                return Ok(cartItemDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetCartItems")]
        public async Task<ActionResult<IEnumerable<CartItemModel>>> GetCartItems()
        {
            try
            {
                //retrieve the users email
                var userIdentity = User.Identity as ClaimsIdentity;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                IEnumerable<CartItemModel> cartItemModels = await _shoppingCartService.GetCartItems(email);

                if (cartItemModels == null || cartItemModels.Count() == 0)
                {
                    return NoContent();
                }

                IEnumerable<ProductModel> productModels = await _productService.GetProducts();

                //Convert to dtos
                IEnumerable<CartItemDTO> cartItemDTOs = cartItemModels.ConvertToCartItemDTOs(productModels);

                if (cartItemDTOs == null || cartItemDTOs.Count() == 0)
                {
                    return NoContent();
                }

                return Ok(cartItemDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch]
        [Authorize]
        [Route("UpdateCartItemQuantity/{id:int}")]
        public async Task<ActionResult<CartItemDTO>> DeleteCartItem(int id, [FromBody] UpdateCartItemQuantityDTO updateCartItemQuantityDTO)
        {
            try
            {
                if (updateCartItemQuantityDTO.CartItemId == 0 || updateCartItemQuantityDTO.Quantity == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                //Convert to model
                CartItemModel cartItemModel = updateCartItemQuantityDTO.ConvertToCartItemMdodel();

                //retrieve the users email
                var userIdentity = User.Identity as ClaimsIdentity;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                cartItemModel = await _shoppingCartService.UpdateCartItemQuantity(cartItemModel, email);

                if (cartItemModel == null || cartItemModel.CartItemId == default)
                {
                    return NotFound();
                }
                else
                {
                    ProductModel productModel = await _productService.GetProduct(cartItemModel.ProductId);

                    CartItemDTO cartItemDTO = cartItemModel.ConvertToCartItemDTO(productModel);
                    
                    return Ok(cartItemDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("DeleteCartItem/{id:int}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            try
            {
                //retrieve the users email
                var userIdentity = User.Identity as ClaimsIdentity;
                string? email = userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                await _shoppingCartService.DeleteCartItem(id, email);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
