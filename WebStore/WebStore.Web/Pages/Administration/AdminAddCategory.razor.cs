using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages.Administration
{
    public partial class AdminAddCategory
    {
        [Inject]
        public IProductService ProductService { get; set; }

        
        private int ListSize { get; set; } = 1;

        private ProductCategoryDTO ProductCategory { get; set; } = new ProductCategoryDTO();

        private List<string> ValidationErrors { get; set; } = new List<string>();

        private List<ProductCategoryDTO> ProductCategories = new List<ProductCategoryDTO>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ProductCategories = (List<ProductCategoryDTO>)await ProductService.GetProductCategoriesAsync();
                ProductCategories.Sort((x, y) => x.CategoryName.CompareTo(y.CategoryName));
                SetListSize();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void SaveCategory_Click()
        {
            //validate category name
            ValidateCategory();

            if (ValidationErrors.Count == 0)
            {
                // check if category already exits
                ProductCategoryDTO? product = ProductCategories.FirstOrDefault(x => x.CategoryName.ToLower().Equals(ProductCategory.CategoryName.ToLower()));
                if (product == default)
                {
                    //Unique so save
                    ProductCategory = await ProductService.AddCategoryAsync(ProductCategory);

                    ProductCategories.Add(ProductCategory);
                    ProductCategories.Sort((x, y) => x.CategoryName.CompareTo(y.CategoryName));

                    SetListSize();

                    StateHasChanged();
                }
                else
                {
                    ValidationErrors.Clear();
                    ValidationErrors.Add(nameof(ProductCategory.CategoryName) + $" This category already exists");
                }
            }
        }

        private void SetListSize()
        {
            if (ProductCategories.Count > 0 && ProductCategories.Count <= 10)
            {
                ListSize = ProductCategories.Count;
            }
            else if (ProductCategories.Count > 10)
            {
                ListSize = 10;
            }
        }

        private void ValidateCategory()
        {
            ValidationErrors.Clear();
            
            ProductCategoryValidator categoryValidator = new ProductCategoryValidator();
            ValidationResult categoryResults = categoryValidator.Validate(ProductCategory);

            if (categoryResults.IsValid == false)
            {
                foreach (var failure in categoryResults.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }
    }
}
