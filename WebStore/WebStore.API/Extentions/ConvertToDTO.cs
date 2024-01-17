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
    }
}
