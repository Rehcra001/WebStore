using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using System;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages.CustomerProfile
{
    public partial class CustomerDetail
    {
        [Inject]
        public ICustomerService CustomerService { get; set; }

        [Inject]
        public IManageCustomerDetailLocalStorage ManageCustomerDetailLocalStorage { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public CustomerDTO Customer { get; set; } = new CustomerDTO();

        private List<AddressDTO> Addresses = new List<AddressDTO>();

        private IList<List<string>> ValidationAddressErrors = new List<List<string>>();

        private List<string> ValidationCustomerDetailErrors = new List<string>();

        private List<string> ValidationNewAddressErrors = new List<string>();

        private List<bool> ShouldDisplayAddress = new List<bool>();
        private List<string> AddressDisplay = new List<string>();

        private bool IsNewAddress = false;
        private string NewAddressDisplay = "none";

        private AddressDTO NewAddress { get; set; } = new AddressDTO();

        protected override void OnInitialized()
        {
            Addresses = Customer.AddressList.ToList();
            //instantiate and error list for each address in Addresses
            foreach (AddressDTO address in Addresses)
            {
                ValidationAddressErrors.Add(new List<string>());
                ShouldDisplayAddress.Add(false);
                AddressDisplay.Add("none");
            }
        }

        private void ShowHideNewAddress()
        {
            IsNewAddress = !IsNewAddress;
            NewAddress = new AddressDTO();
            ValidationNewAddressErrors.Clear();
            if (IsNewAddress)
            {
                NewAddressDisplay = "";
            }
            else
            {
                NewAddressDisplay = "none";
            }
        }

        private async Task SaveCustomerDetail()
        {
            //Validate
            ValidateCustomerDetail();

            if (ValidationCustomerDetailErrors.Count == 0)
            {
                UpdateCustomerDetailDTO updateCustomerDetailDTO = new UpdateCustomerDetailDTO
                {
                    CustomerId = Customer.CustomerId,
                    FirstName = Customer.FirstName,
                    LastName = Customer.LastName,
                    PhoneNumber = Customer.PhoneNumber,
                    EmailAddress = Customer.EmailAddress
                };

                await CustomerService.UpdateCustomerDetailAsync(updateCustomerDetailDTO);

                await ManageCustomerDetailLocalStorage.SaveCustomerDetail(Customer);
            }

        }

        private async Task SaveAddress(int index)
        {
            ValidateCustomerAddress(index);

            if (ValidationAddressErrors[index].Count == 0)
            {
                //Save address
                await CustomerService.UpdateCustomerAddressAsync(Addresses[index]);

                Customer.AddressList = Addresses;

                await ManageCustomerDetailLocalStorage.SaveCustomerDetail(Customer);
            }
        }

        private async Task AddNewAddress()
        {
            ValidateNewAddress();

            if (ValidationNewAddressErrors.Count == 0)
            {
                //Save new address
                AddressDTO address = await CustomerService.AddCustomerAddressAsync(NewAddress);
                if (address == null || address.AddressId == 0)
                {
                    throw new Exception("Unable to save new Address");
                }
                else
                {
                    Addresses.Add(address);
                    Customer.AddressList = Addresses;
                    await ManageCustomerDetailLocalStorage.SaveCustomerDetail(Customer);

                    //Add validation for new address
                    ValidationAddressErrors.Add(new List<string>());
                    ShouldDisplayAddress.Add(false);
                    AddressDisplay.Add("none");

                    //Clear new address
                    NewAddress = new AddressDTO();
                }

            }
        }

        private void ValidateCustomerDetail()
        {
            ValidationCustomerDetailErrors.Clear();

            CustomerDetailValidator validationRules = new CustomerDetailValidator();
            ValidationResult validationResult = validationRules.Validate(Customer);

            if (validationResult.IsValid == false)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ValidationCustomerDetailErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }

        private void ValidateCustomerAddress(int index)
        {
            ValidationAddressErrors[index].Clear();

            AddressValidator validationRules = new AddressValidator();
            ValidationResult validationResult = validationRules.Validate(Addresses[index]);

            if (validationResult.IsValid == false)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ValidationAddressErrors[index].Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }

        private void ValidateNewAddress()
        {
            ValidationNewAddressErrors.Clear();

            AddressValidator validationRules = new AddressValidator();
            ValidationResult validationResult = validationRules.Validate(NewAddress);

            if (validationResult.IsValid == false)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ValidationNewAddressErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }

        private void ShowHideAddress(int index)
        {
            if (AddressDisplay[index] == "none")
            {
                AddressDisplay[index] = "";
            }
            else
            {
                AddressDisplay[index] = "none";
            }
        }

    }
}
