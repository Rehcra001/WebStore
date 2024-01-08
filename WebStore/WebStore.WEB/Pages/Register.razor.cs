using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;
using FluentValidation.Results;

namespace WebStore.WEB.Pages
{
    public partial class Register
    {
        [Inject]
        public IRegistrationService RegistrationService { get; set; }

        private UserRegistrationDTO _userToRegister = new UserRegistrationDTO();
        private AddressDTO _address = new AddressDTO();
        private bool _registerSuccessful = false;
        private bool _attemptToregisterFailed = false;
        private string? _attemptToRegisterFailedMessage = null;

        public List<string> ValidationErrors { get; set; } = new List<string>();


        // validate data
        private void RegisterUser_Click()
        {
            ValidateUser();
            ValidateAddress();
        }

        private void ValidateUser()
        {
            UserRegistrationValidator _userValidator = new UserRegistrationValidator();
            ValidationResult userResults = _userValidator.Validate(_userToRegister);
            ValidationErrors.Clear();

            if (userResults.IsValid == false)
            {
                foreach (ValidationFailure failure in userResults.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }

        private void ValidateAddress()
        {
            AddressValidator _addressValidator = new AddressValidator();
            ValidationResult addressResults = _addressValidator.Validate(_address);

            if (addressResults.IsValid == false)
            {
                foreach (ValidationFailure failure in addressResults.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }
    }
}
