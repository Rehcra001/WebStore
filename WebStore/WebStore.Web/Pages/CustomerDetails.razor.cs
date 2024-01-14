using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using WebStore.DTO;
using WebStore.WEB.Providers;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class CustomerDetails
    {
        [Inject]
        ICustomerService CustomerService { get; set; }


        public string ErrorMessage { get; set; }

        public CustomerDTO Customer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Customer = await CustomerService.GetCustomerDetailsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

    }
}
