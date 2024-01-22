using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages.Administration
{
    public partial class AdminProductDetail
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Parameter]
        public string Heading { get; set; } = "Heading";

        [Parameter]
        public int ProductId { get; set; } = 0;

        public ProductDTO Product { get; set; } = new ProductDTO();

        public IEnumerable<ProductCategoryDTO> ProductCategories { get; set; } = new List<ProductCategoryDTO>();

        public IEnumerable<UnitPerDTO> UnitPers { get; set; } = new List<UnitPerDTO>();

        private IBrowserFile Image { get; set; }

        private string ImageDataUrl { get; set; } = string.Empty;

        private byte[] ImageBytes { get; set; }

        private List<string> ValidationErrors { get; set; } = new List<string>();

        private string _showSaveButton = "";


        protected override async Task OnInitializedAsync()
        {
            if (ProductId == 0)
            {
                //New Product
                ProductCategories = await ProductService.GetProductCategories();
                UnitPers = await ProductService.GetUnitPers();
            }
            else
            {
                //Existing Product

                //Product = await ProductService.GetProduct(ProductId);
                ProductCategories = await ProductService.GetProductCategories();
                UnitPers = await ProductService.GetUnitPers();

                //Get image byte[] and convert to ImageDataUrl

            }            
        }

        private async Task SaveProduct_Click()
        {
            // add image
            if (ImageBytes != null)
            {
                Product.Picture = ImageBytes;
            }
            
            // unit per name
            if (UnitPers.Any(x => x.UnitPerId == Product.UnitPerId))
            {
                Product.UnitPer = UnitPers.First(x => x.UnitPerId == Product.UnitPerId).UnitPer;
            }

            if (ProductCategories.Any(x => x.ProductCategoryId == Product.CategoryId))
            {
                Product.CategoryName = ProductCategories.First(x => x.ProductCategoryId == Product.CategoryId).CategoryName;
            }

            // Validate
            ValidateProduct();

            //Save
            if (ValidationErrors.Count == 0)
            {
                if (Product.ProductId == 0)
                {
                    Product = await ProductService.AddProductAsync(Product);

                    _showSaveButton = "none";
                }
                else
                {
                    // Update Product
                }
                
            }
            
        }

        private void ValidateProduct()
        {
            ProductValidator productValidator = new ProductValidator();
            ValidationResult productResults = productValidator.Validate(Product);
            ValidationErrors.Clear();

            if (productResults.IsValid == false)
            {
                foreach (ValidationFailure failure in productResults.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
