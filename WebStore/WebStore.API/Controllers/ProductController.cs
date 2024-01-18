﻿using Microsoft.AspNetCore.Authorization;
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

    }


}
