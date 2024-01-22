using Microsoft.AspNetCore.Components;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages.Administration
{
    public partial class Administration
    {
        [Inject]
        public IProductService ProductService { get; set; }


        private string? PageToView { get; set; }

        private string[] AddNew = new string[] {"NewProduct", "NewCategory", "NewUnitPer"};
        private string[] EditExisting = new string[] { "EditProduct", "EditCategory", "EditUnitPer", "EditHomePage" };


        private void Menu_Click(string pageToView)
        {
            PageToView = pageToView;
        }

    }
}
