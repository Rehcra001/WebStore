using Microsoft.AspNetCore.Components;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages.Administration
{
    public partial class Administration
    {
        [Inject]
        public IManageAdminShippedOrdersLocalStorageService ManageAdminShippedOrdersLocalStorageService { get; set; }
        [Inject]
        public IManageAdminPaymentOrdersLocalStorageService ManageAdminPaymentOrdersLocalStorageService { get; set; }

        private string? PageToView { get; set; }

        private string[] Orders = new string[] { "PaymentConfirmation", "ShippedConfirmation" };
        private string[] AddNew = new string[] {"NewProduct", "NewCategory", "NewUnitPer", "NewCompanyDetail"};
        private string[] EditExisting = new string[] { "EditProduct", "EditCategory", "EditUnitPer", "EditHomePage", "EditCompanyDetail" };

        protected override async Task OnInitializedAsync()
        {
            await ManageAdminPaymentOrdersLocalStorageService.GetCollection();
            await ManageAdminShippedOrdersLocalStorageService.GetCollection();
        }


        private void Menu_Click(string pageToView)
        {
            PageToView = pageToView;
        }

    }
}
