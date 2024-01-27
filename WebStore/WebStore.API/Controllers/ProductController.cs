using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
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

                if (productCategoryModel is null || productCategoryModel.ProductCategoryId == default)
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

        [HttpPost]
        [AllowAnonymous]
        [Route("AddUnitPer")]
        public async Task<ActionResult<UnitPerDTO>> AddUnitPer([FromBody] UnitPerDTO unitPerDTO)
        {
            try
            {
                //Convert to model
                UnitPerModel unitPerModel = unitPerDTO.ConvertToUnitPerModel();
                //Validate model
                var unitPerErrors = ValidationHelper.Validate(unitPerModel);
                if (unitPerErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, unitPerErrors);
                }

                //Save model
                unitPerModel = await _productService.AddUnitPer(unitPerModel);

                if (unitPerModel is null || unitPerModel.UnitPerId == default)
                {
                    return NoContent();
                }
                else
                {
                    //convert to DTO
                    unitPerDTO = unitPerModel.ConvertToUnitPerDTO();
                    return CreatedAtAction(nameof(GetUnitPer), new { id = unitPerDTO.UnitPerId }, unitPerDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetUnitPer/{id:int}")]
        public async Task<ActionResult<UnitPerDTO>> GetUnitPer(int id)
        {
            try
            {
                UnitPerModel unitPerModel = await _productService.GetUnitPer(id);

                if (unitPerModel == null || unitPerModel.UnitPerId == default)
                {
                    return NoContent();
                }
                else
                {
                    //convert to dto
                    UnitPerDTO unitPerDTO = unitPerModel.ConvertToUnitPerDTO();
                    return Ok(unitPerDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddProduct")]
        public async Task<ActionResult<ProductDTO>> AddProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                //convert to model
                ProductModel productModel = productDTO.ConvertToProductModel();
                //Validate
                var productErrors = ValidationHelper.Validate(productModel);
                if (productErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, productErrors);
                }

                //save model
                productModel = await _productService.AddProduct(productModel);

                if (productModel == null || productModel.ProductId == default)
                {
                    return NoContent();
                }
                else
                {
                    productDTO = productModel.ConvertToProductDto();
                    return CreatedAtAction(nameof(GetProduct), new { id = productDTO.ProductId }, productDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetProduct/{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            try
            {
                ProductModel productModel = await _productService.GetProduct(id);

                if (productModel == null || productModel.ProductId == default)
                {
                    return NoContent();
                }
                else
                {
                    //Convert to Dto
                    ProductDTO productDTO = productModel.ConvertToProductDto();
                    return Ok(productDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetProductCategories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryDTO>>> GetProductCategories()
        {
            try
            {
                IEnumerable<ProductCategoryModel> productCategoryModels = await _productService.GetAllCatergories();

                if (productCategoryModels == null || productCategoryModels.Count() == 0)
                {
                    return NoContent();
                }
                else
                {
                    IEnumerable<ProductCategoryDTO> productCategoryDTOs = productCategoryModels.ConvertToProductCategoriesDTO();

                    return Ok(productCategoryDTOs);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetUnitPers")]
        public async Task<ActionResult<IEnumerable<UnitPerDTO>>> GetUnitPers()
        {
            try
            {
                IEnumerable<UnitPerModel> unitPerModels = await _productService.GetAllUnitPers();

                if (unitPerModels == null || unitPerModels.Count() == 0)
                {
                    return NoContent();
                }
                else
                {
                    IEnumerable<UnitPerDTO> unitPerDTOs = unitPerModels.ConvertToUnitPersDTO();

                    return Ok(unitPerDTOs);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                IEnumerable<ProductModel> productModels = await _productService.GetProducts();

                if (productModels == null || productModels.Count() == 0)
                {
                    return NoContent();
                }
                else
                {
                    IEnumerable<ProductDTO> productDTOs = productModels.ConvertToProductsDTO();
                    return Ok(productDTOs);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("UpdateProduct")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(ProductDTO productDTO)
        {
            try
            {      
                //Convert to model
                ProductModel productModel = productDTO.ConvertToProductModel();

                //validate the data
                var productErrors = ValidationHelper.Validate(productDTO);
                if (productErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, productErrors);
                }

                productModel = await _productService.UpdateProduct(productModel);

                if (productModel == null || productModel.ProductId == 0)
                {
                    //product was not saved
                    return NoContent();
                }
                else
                {
                    return Ok(productDTO);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("UpdateProductCategory")]
        public async Task<ActionResult<ProductCategoryDTO>> UpdateProductCategory(ProductCategoryDTO productCategoryDTO)
        {
            try
            {
                //Convert to model
                ProductCategoryModel productCategoryModel = productCategoryDTO.ConvertToProductCategoryModel();

                //validate
                var categoryErrors = ValidationHelper.Validate(productCategoryModel);
                if (categoryErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, categoryErrors);
                }

                //save
                productCategoryModel = await _productService.UpdateProductCategory(productCategoryModel);
                if (productCategoryModel == null || productCategoryModel.ProductCategoryId == 0)
                {
                    return NoContent();
                }

                //Convert to DTO
                productCategoryDTO =  productCategoryModel.ConvertToProductCategoryDTO();
                return Ok(productCategoryDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("UpdateUnitPer")]
        public async Task<ActionResult<UnitPerDTO>> UpdateUnitPer(UnitPerDTO unitPerDTO)
        {
            try
            {
                //convert to model
                UnitPerModel unitPerModel = unitPerDTO.ConvertToUnitPerModel();

                //validate
                var unitPerErrors = ValidationHelper.Validate(unitPerModel);
                if (unitPerErrors.Count > 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, unitPerErrors);
                }

                //save
                unitPerModel = await _productService.UpdateUnitPer(unitPerModel);
                if (unitPerModel == null || unitPerModel.UnitPerId == 0)
                {
                    return NoContent();
                }

                unitPerDTO = unitPerModel.ConvertToUnitPerDTO();
                return Ok(unitPerDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }


}
