﻿using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Extentions
{
    public static class ConvertToDTO
    {
        public static CustomerDTO ConvertToCustomerDTO(this CustomerModel customerModel)
        {
            CustomerDTO customerDTO = new CustomerDTO
            {
                CustomerId = customerModel.CustomerId,
                FirstName = customerModel.FirstName,
                LastName = customerModel.LastName,
                EmailAddress = customerModel.EmailAddress,
                PhoneNumber = customerModel.PhoneNumber
            };

            List<AddressDTO> addresses = new List<AddressDTO>();
            foreach(AddressModel address in customerModel.AddressList)
            {
                AddressDTO addressDTO = new AddressDTO();

                addressDTO.AddressId = address.AddressId;
                addressDTO.AddressLine1 = address.AddressLine1;
                addressDTO.AddressLine2 = address.AddressLine2;
                addressDTO.Suburb = address.Suburb;
                addressDTO.City = address.City;
                addressDTO.PostalCode = address.PostalCode;
                addressDTO.Country = address.Country;
                addressDTO.CustomerId = address.CustomerId;

                addresses.Add(addressDTO);
            }

            customerDTO.AddressList = addresses;
            return customerDTO;
        }

        public static ProductCategoryDTO ConvertToProductCategoryDTO(this ProductCategoryModel productCategoryModel)
        {
            return (new ProductCategoryDTO
            {
                ProductCategoryId = productCategoryModel.ProductCategoryId,
                CategoryName = productCategoryModel.CategoryName
            });
        }

        public static UnitPerDTO ConvertToUnitPerDTO(this UnitPerModel unitPerModel)
        {
            return (new UnitPerDTO
            {
                UnitPerId = unitPerModel.UnitPerId,
                UnitPer = unitPerModel.UnitPer
            });
        }

        public static ProductDTO ConvertToProductDto(this ProductModel productModel)
        {
            return (new ProductDTO
            {
                ProductId = productModel.ProductId,
                Name = productModel.Name,
                Description = productModel.Description,
                Picture = productModel.Picture,
                Price = productModel.Price,
                QtyInStock = productModel.QtyInStock,
                UnitPerId = productModel.UnitPerId,
                UnitPer = productModel.UnitPer,
                CategoryId = productModel.CategoryId,
                CategoryName = productModel.CategoryName
            });
        }

        public static IEnumerable<ProductCategoryDTO> ConvertToProductCategoriesDTO (this IEnumerable<ProductCategoryModel> productCategoryModels)
        {
            return (from product in productCategoryModels
                    select new ProductCategoryDTO
                    {
                        ProductCategoryId = product.ProductCategoryId,
                        CategoryName = product.CategoryName
                    });
        }

        public static IEnumerable<UnitPerDTO> ConvertToUnitPersDTO(this IEnumerable<UnitPerModel> unitPerModels)
        {
            return (from unitPer in unitPerModels
                    select new UnitPerDTO
                    {
                        UnitPerId = unitPer.UnitPerId,
                        UnitPer = unitPer.UnitPer
                    });
        }

        public static IEnumerable<ProductDTO> ConvertToProductsDTO(this IEnumerable<ProductModel> productModels)
        {
            return (from product in productModels
                    select new ProductDTO
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Description = product.Description,
                        Picture = product.Picture,
                        Price = product.Price,
                        QtyInStock = product.QtyInStock,
                        UnitPerId = product.UnitPerId,
                        UnitPer = product.UnitPer,
                        CategoryId = product.CategoryId,
                        CategoryName = product.CategoryName
                    });
        }
    }
}
