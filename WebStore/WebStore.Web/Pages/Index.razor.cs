using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using WebStore.DTO;
using WebStore.WEB.Providers;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class Index
    {
        [Inject]
        public IProductService ProductService { get; set; }

        private List<ProductCategoryDTO> ProductCategories { get; set; } = new List<ProductCategoryDTO>();

        private string WebstoreTitle { get; set; } = "Web Store";

        protected override async Task OnInitializedAsync()
        {
            var productCategories = await ProductService.GetProductCategoriesAsync();

            if (productCategories.Count() > 0)
            {
                ProductCategories = (List<ProductCategoryDTO>)productCategories;
            }
        }
    }
}

