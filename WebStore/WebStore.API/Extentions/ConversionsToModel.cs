﻿using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Extentions
{
    public static class ConversionsToModel
    {
        public static CustomerModel ConvertToCustomerModel(this UserRegistrationDTO userRegistrationDTO)
        {
            CustomerModel customer = new CustomerModel();
            List<AddressModel> addresses = new List<AddressModel>();

            //No customer or address id's exist at this point
            customer.FirstName = userRegistrationDTO.FirstName;
            customer.LastName = userRegistrationDTO.LastName;
            customer.EmailAddress = userRegistrationDTO.EmailAddress;
            customer.PhoneNumber = userRegistrationDTO.PhoneNumber;

            foreach (AddressDTO addressDTO in userRegistrationDTO.AddressList)
            {
                AddressModel addressModel = new AddressModel
                {
                    AddressLine1 = addressDTO.AddressLine1,
                    AddressLine2 = addressDTO.AddressLine2,
                    Suburb = addressDTO.Suburb,
                    City = addressDTO.City,
                    PostalCode = addressDTO.PostalCode,
                    Country = addressDTO.Country
                };

                addresses.Add(addressModel);
            }

            customer.AddressList = addresses;

            return customer;
        }

        public static UserRegistrationModel ConvertToUserRegistrationModel(this UserRegistrationDTO userRegistrationDTO)
        {
            return (new UserRegistrationModel
            {
                EmailAddress = userRegistrationDTO.EmailAddress,
                ConfirmEmailAddress = userRegistrationDTO.ConfirmEmailAddress,
                Password = userRegistrationDTO.Password,
                ConfirmPassword = userRegistrationDTO.ConfirmPassword
            });

        }
    }
}
