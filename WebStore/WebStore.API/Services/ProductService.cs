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

        public async Task<ProductModel> AddProduct(ProductModel product)
        {
            try
            {
                return await _productRepository.AddProduct(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        public async Task<IEnumerable<ProductCategoryModel>> GetAllCatergories()
        {
            try
            {
                return await _productRepository.GetAllCatergories();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<UnitPerModel>> GetAllUnitPers()
        {
            try
            {
                return await _productRepository.GetAllUnitPers();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            try
            {
                return await _productRepository.GetProduct(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        public async Task<IEnumerable<ProductModel>> GetProducts()
        {
            try
            {
                return await _productRepository.GetProducts();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ProductListModel>> GetProductsList()
        {
            try
            {
                return await _productRepository.GetProductsList();
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

        public async Task<ProductModel> UpdateProduct(ProductModel product)
        {
            try
            {
                return await _productRepository.UpdateProduct(product);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductCategoryModel> UpdateProductCategory(ProductCategoryModel productCategory)
        {
            try
            {
                return await _productRepository.UpdateProductCategory(productCategory);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UnitPerModel> UpdateUnitPer(UnitPerModel unitPer)
        {
            try
            {
                return await _productRepository.UpdateUnitPer(unitPer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
