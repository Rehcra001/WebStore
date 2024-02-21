using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class Products
    {
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorage { get; set; }

        [Parameter]
        public string Category { get; set; } = string.Empty;

        private List<ProductDTO> ProductList { get; set; } = new List<ProductDTO>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var products = await ManageProductsLocalStorage.GetCollection();

                if (products.Count() > 0)
                {
                    ProductList = products.Where(x => x.CategoryName.Equals(Category)).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
