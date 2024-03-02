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

namespace WebStore.WEB.Pages.CustomerProfile
{
    public partial class CustomerProfile
    {
        [Inject]
        public IManageCustomerDetailLocalStorage ManageCustomerDetailLocalStorage { get; set; }

        [Inject]
        public IManageCustomerOrdersLocalStorage ManageCustomerOrdersLocalStorage { get; set; }

        private string ErrorMessage { get; set; } = string.Empty;
        private string? PageToView { get; set; }
        private string[] CustomerDetail = new string[] { "CustomerDetail" };
        private string[] CustomerOrders = new string[] { "PaymentOutstanding", "ShippingOutstanding", "Completed" };
        


        protected override async Task OnInitializedAsync()
        {
            try
            {
                //Clear local storage
                await ManageCustomerDetailLocalStorage.RemoveCustomerDetail();
                await ManageCustomerOrdersLocalStorage.RemoveCollection();

                //Retrieve all customer info
                await ManageCustomerDetailLocalStorage.GetCustomerDetail();
                await ManageCustomerOrdersLocalStorage.GetCollection();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void Menu_Click(string pageToView)
        {
            PageToView = pageToView;
        }

    }
}
