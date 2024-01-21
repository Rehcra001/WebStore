using Microsoft.AspNetCore.Components;

namespace WebStore.WEB.Pages.Administration
{
    public partial class Administration
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string? PageToView { get; set; }

        private string[] AddNew = new string[] {"NewProduct", "NewCategory", "NewUnitPer"};
        private string[] EditExisting = new string[] { "EditProduct", "EditCategory", "EditUnitPer", "EditHomePage" };


        private void Menu_Click(string pageToView)
        {
            PageToView = pageToView;
        }

    }
}
