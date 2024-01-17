using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.API.Extentions;
using WebStore.API.Services.Contracts;
using WebStore.API.ValidationClasses;
using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddProductCategory")]
        public async Task<ActionResult<ProductCategoryDTO>> AddProductCategory([FromBody] ProductCategoryDTO productCategoryDTO)
        {
            try
            {
                //Convert to ProductCategoryModel
                ProductCategoryModel productCategoryModel = productCategoryDTO.ConvertToProductCategoryModel();

                //Validate data
                var productCategoryErrors = ValidationHelper.Validate(productCategoryModel);
                if (productCategoryErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, productCategoryErrors);
                }

                productCategoryModel = await _productService.AddProductCategory(productCategoryModel);

                if (productCategoryModel.ProductCategoryId == default)
                {
                    return NoContent();
                }

                productCategoryDTO = productCategoryModel.ConvertToProductCategoryDTO();

                //return Ok(productCategoryDTO);
                return CreatedAtAction(nameof(GetProductCategory), new { id = productCategoryDTO.ProductCategoryId }, productCategoryDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetProductCategory/{id:int}")]
        public async Task<ActionResult<ProductCategoryDTO>> GetProductCategory(int id)
        {
            try
            {
                ProductCategoryModel productCategoryModel = await _productService.GetProductCategory(id);

                if (productCategoryModel.ProductCategoryId == default)
                {
                    return NoContent();
                }
                else
                {
                    ProductCategoryDTO productCategoryDTO = productCategoryModel.ConvertToProductCategoryDTO();
                    return Ok(productCategoryDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

    
}
