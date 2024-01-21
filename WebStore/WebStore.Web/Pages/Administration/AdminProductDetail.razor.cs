using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Drawing;
using System.Security.AccessControl;
using WebStore.DTO;

namespace WebStore.WEB.Pages.Administration
{
    public partial class AdminProductDetail
    {
        [Parameter]
        public string Heading { get; set; } = "Heading";

        private IBrowserFile Image { get; set; }

        private string ImageDataUrl { get; set; } = string.Empty;

        private byte[] ImageBytes { get; set; }

        private ProductDTO Product { get; set; } = new ProductDTO();

        private List<string> ValidationErrors { get; set; } = new List<string>();





        private async Task PreviewImage(InputFileChangeEventArgs e)
        {
            ImageDataUrl = string.Empty;
            //Image = e.File;
            Image = await e.File.RequestImageFileAsync("image/png", 200, 200);

            try
            {

                using var file = Image.OpenReadStream();
                long len = file.Length;
                ImageBytes = new byte[len];
                await file.ReadAsync(ImageBytes, 0, (int)file.Length);

                ImageDataUrl = "data:image;base64," + Convert.ToBase64String(ImageBytes);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
