using WebStore.DTO;
using WebStore.Models;

namespace WebStore.API.Extentions
{
    public static class ConvertToModel
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

        public static UserSignInModel ConvertToUserSignInModel(this UserSignInDTO userSignInDTO)
        {
            return (new UserSignInModel
            {
                EmailAddress = userSignInDTO.EmailAddress,
                Password = userSignInDTO.Password
            });
        }

        public static ProductCategoryModel ConvertToProductCategoryModel(this ProductCategoryDTO productCategoryDTO)
        {
            return (new ProductCategoryModel
            {
                ProductCategoryId = productCategoryDTO.ProductCategoryId,
                CategoryName = productCategoryDTO.CategoryName,
                Picture = productCategoryDTO.Picture                
            });
        }

        public static UnitPerModel ConvertToUnitPerModel(this UnitPerDTO unitPerDTO)
        {
            return (new UnitPerModel
            {
                UnitPerId = unitPerDTO.UnitPerId,
                UnitPer = unitPerDTO.UnitPer
            });
        }

        public static ProductModel ConvertToProductModel(this ProductDTO productDTO)
        {
            return (new ProductModel
            {
                ProductId = productDTO.ProductId,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Picture = productDTO.Picture,
                Price = productDTO.Price,
                QtyInStock = productDTO.QtyInStock,
                UnitPerId = productDTO.UnitPerId,
                UnitPer = productDTO.UnitPer,
                CategoryId = productDTO.CategoryId,
                CategoryName = productDTO.CategoryName
            });
        }

        public static CartItemModel ConvertToCartItemModel(this CartItemAddToDTO cartItemDTO)
        {
            return (new CartItemModel
            {
                ProductId = cartItemDTO.ProductId,
                Quantity = cartItemDTO.Quantity
            });
        }

        public static CartItemModel ConvertToCartItemMdodel(this UpdateCartItemQuantityDTO updateCartItemQuantityDTO)
        {
            return (new CartItemModel
            {
                CartItemId = updateCartItemQuantityDTO.CartItemId,
                Quantity = updateCartItemQuantityDTO.Quantity
            });
        }

        public static AddressModel ConvertToAddressModel(this AddressDTO addressDTO)
        {
            return (new AddressModel
            {
                AddressId = addressDTO.AddressId,
                AddressLine1 = addressDTO.AddressLine1,
                AddressLine2 = addressDTO.AddressLine2,
                Suburb = addressDTO.Suburb,
                City = addressDTO.City,
                PostalCode = addressDTO.PostalCode,
                Country = addressDTO.Country,
                CustomerId = addressDTO.CustomerId
            });
        }

        public static CompanyEFTDetailModel ConvertToEFTModel(this CompanyEFTDTO companyEFTDTO)
        {
            return (new CompanyEFTDetailModel
            {
                EFTId = companyEFTDTO.EFTId,
                Bank = companyEFTDTO.Bank,
                AccountType = companyEFTDTO.AccountType,
                AccountNumber = companyEFTDTO.AccountNumber,
                BranchCode = companyEFTDTO.BranchCode
            });
        }

        public static (CompanyDetailModel CompanyDetailModel, CompanyEFTDetailModel CompanyEFTDetailModel, AddressModel CompanyAddressModel) ConvertToCompanyDetailModel(this CompanyDetailDTO companyDetailDTO)
        {
            CompanyDetailModel companyDetailModel = new CompanyDetailModel
            {
                CompanyId = companyDetailDTO.CompanyId,
                CompanyName = companyDetailDTO.CompanyName,
                CompanyLogo = companyDetailDTO.CompanyLogo,
                PhoneNumber = companyDetailDTO.PhoneNumber,
                EmailAddress = companyDetailDTO.EmailAddress,
                AddressId = companyDetailDTO.AddressId,
                EFTId = companyDetailDTO.EFTId
            };

            CompanyEFTDetailModel companyEFTDetailModel = companyDetailDTO.CompanyEFT.ConvertToEFTModel();

            AddressModel companyAddressModel = companyDetailDTO.CompanyAddress.ConvertToAddressModel();

            return (companyDetailModel, companyEFTDetailModel, companyAddressModel);
        }

        public static CompanyDetailModel ConvertToCompanyDetailModel(this UpdateCompanyDetailDTO updateCompanyDetailDTO)
        {
            return (new CompanyDetailModel
            {
                CompanyId = updateCompanyDetailDTO.CompanyId,
                CompanyName = updateCompanyDetailDTO.CompanyName,
                CompanyLogo = updateCompanyDetailDTO.CompanyLogo,
                PhoneNumber = updateCompanyDetailDTO.PhoneNumber,
                EmailAddress = updateCompanyDetailDTO.EmailAddress
            });
        }
    }
}
