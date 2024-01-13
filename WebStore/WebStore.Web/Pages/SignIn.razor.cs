using Blazored.LocalStorage;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using WebStore.DTO;
using WebStore.WEB.Providers;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages
{
    public partial class SignIn
    {
        [Inject]
        HttpClient HttpClient { get; set; }

        [Inject]
        ISignInService SignInService { get; set; }

        [Inject]
        ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        private UserSignInDTO _userToSignIn = new UserSignInDTO();
        private bool _signInSuccessful = false;
        private bool _attemptToSignInFailed = false;

        public List<string> ValidationErrors { get; set; } = new List<string>();

        private async Task SignInUser()
        {
            ValidateUserSignIn();

            if (ValidationErrors.Count == 0)
            {
                _attemptToSignInFailed = false;
                var signInStatus = await SignInService.SignIn(_userToSignIn);

                if (signInStatus.IsSuccessful)
                {
                    await LocalStorageService.SetItemAsync("bearerToken", signInStatus.jsonToken);

                    await ((AppAuthenticationStateProvider)AuthenticationStateProvider).SignIn();

                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", signInStatus.jsonToken);

                    _signInSuccessful = true;

                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    _attemptToSignInFailed = true;
                }
            }
        }

        private void ValidateUserSignIn()
        {
            UserSignInValidator validations = new UserSignInValidator();
            ValidationResult validationResult = validations.Validate(_userToSignIn);
            ValidationErrors.Clear();

            if (validationResult.IsValid == false)
            {
                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }

        }
    }
}
