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

        public IBrowserFile Image { get; set; }

        private void GetPathString(InputFileChangeEventArgs e)
        {
            Image = e.File;
        }

        private async Task AddProduct_Click()
        {
            byte[] bytes;
            try
            {
                using var file = Image.OpenReadStream();
                long len = file.Length;
                bytes = new byte[len];
                await file.ReadAsync(bytes, 0, (int)file.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            ProductDTO productDTO = new ProductDTO
            {
                Name = "Table",
                Description = "It has four legs",
                Picture = bytes,
                Price = 10M,
                QtyInStock = 1,
                UnitPerId = 1,
                UnitPer = "kg",
                CategoryId = 1,
                CategoryName = "Biltong"
            };

            productDTO = await ProductService.AddProductAsync(productDTO);
        }
    }
}
