namespace WebStore.WEB.Pages.Administration
{
    public partial class Administration
    {        
        private string? PageToView { get; set; }

        private string[] AddNew = new string[] {"NewProduct", "NewCategory", "NewUnitPer"};
        private string[] EditExisting = new string[] { "EditProduct", "EditCategory", "EditUnitPer", "EditHomePage" };


        private void Menu_Click(string pageToView)
        {
            PageToView = pageToView;
        }

    }
}
