using System;
using WebStore.API.Services.Contracts;
using WebStore.Models;
using WebStore.Repository.Contracts;

namespace WebStore.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<ProductModel> AddProduct(ProductModel product)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductCategoryModel> AddProductCategory(ProductCategoryModel productCategory)
        {
            try
            {
                return await _productRepository.AddProductCategory(productCategory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UnitPerModel> AddUnitPer(UnitPerModel unitPer)
        {
            try
            {
                return await _productRepository.AddUnitPer(unitPer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<ProductModel> GetProduct(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductCategoryModel> GetProductCategory(int id)
        {
            try
            {
                return await _productRepository.GetProductCategory(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UnitPerModel> GetUnitPer(int id)
        {
            try
            {
                return await _productRepository.GetUnitPer(id);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
