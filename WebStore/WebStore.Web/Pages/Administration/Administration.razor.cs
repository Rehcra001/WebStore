namespace WebStore.WEB.Pages.Administration
{
    public partial class Administration
    {        
        private string? PageToView { get; set; }

        private string[] AddNew = new string[] {"NewProduct", "NewCategory", "NewUnitPer", "NewCompanyDetail"};
        private string[] EditExisting = new string[] { "EditProduct", "EditCategory", "EditUnitPer", "EditHomePage", "EditCompanyDetail" };


        private void Menu_Click(string pageToView)
        {
            PageToView = pageToView;
        }

    }
}
