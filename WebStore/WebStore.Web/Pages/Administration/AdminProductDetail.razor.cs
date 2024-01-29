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
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Heading { get; set; } = "Heading";

        [Parameter]
        public string ActionType { get; set; } = string.Empty;

        private int ProductId { get => productId; set { productId = value; ShowSelectedProduct(value); } }
        
        private ProductDTO Product { get; set; } = new ProductDTO();

        private IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();

        private IEnumerable<ProductCategoryDTO> ProductCategories { get; set; } = new List<ProductCategoryDTO>();

        private IEnumerable<UnitPerDTO> UnitPers { get; set; } = new List<UnitPerDTO>();

        private IBrowserFile Image { get; set; }

        private string ImageDataUrl { get; set; } = string.Empty;

        private byte[] ImageBytes { get; set; }
        
        private List<string> ValidationErrors { get; set; } = new List<string>();

        private string _showSaveButton = "";
        private int productId;

        protected override async Task OnInitializedAsync()
        {
            if (String.IsNullOrWhiteSpace(ActionType))
            {
                throw new Exception("ActionType parameter required ('AddAction' or 'EditAction'");
            }
            else
            {
                switch (ActionType)
                {
                    case "AddAction":
                        //New Product
                        ProductCategories = await ProductService.GetProductCategoriesAsync();
                        UnitPers = await ProductService.GetUnitPersAsync();
                        break;
                    case "EditAction":
                        //Existing Product

                        Products = await ProductService.GetProductsAsync();
                        ProductCategories = await ProductService.GetProductCategoriesAsync();
                        UnitPers = await ProductService.GetUnitPersAsync();
                        break;
                }
            }
        }

        private async Task SaveProduct_Click()
        {
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
                if (ActionType.Equals("AddAction"))
                {
                    Product = await ProductService.AddProductAsync(Product);

                    _showSaveButton = "none";
                }
                else if (ActionType.Equals("EditAction"))
                {
                    // Update Product
                    Product = await ProductService.UpdateProductAsync(Product);
                }
            }
        }

        private void ValidateProduct()
        {
            ValidationErrors.Clear();
            ProductValidator productValidator = new ProductValidator();
            ValidationResult productResults = productValidator.Validate(Product);
            

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

                Product.Picture = ImageBytes;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ShowSelectedProduct(int value)
        {
            if (value != 0)
            {
                Product = Products.First(x => x.ProductId == value);
                ImageDataUrl = "data:image;base64," + Convert.ToBase64String(Product.Picture);

            }
        }
    }
}
