using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages.Administration
{
    public partial class AdminAddUnitPer
    {
        [Inject]
        public IProductService ProductService { get; set; }
        private UnitPerDTO UnitPer { get; set; } = new UnitPerDTO();
        private List<string> ValidationErrors { get; set; } = new List<string>();
        private List<UnitPerDTO> UnitPers { get; set; } = new List<UnitPerDTO>();
        private int ListSize { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                UnitPers = (List<UnitPerDTO>)await ProductService.GetUnitPersAsync();
                UnitPers.Sort((x, y) => x.UnitPer.CompareTo(y.UnitPer));

                SetListSize();
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task SaveUnitPer()
        {
            //valdiate
            ValidateUnitPer();

            if (ValidationErrors.Count == 0)
            {
                //save
                UnitPer = await ProductService.AddUnitPerAsync(UnitPer);

                if (UnitPer == null || UnitPer.UnitPerId == 0)
                {
                    throw new Exception("Unable to save the Unit Per");
                }

                UnitPers.Add(UnitPer);
                UnitPers.Sort((x , y) => x.UnitPer.CompareTo(y.UnitPer));

                SetListSize();
            }

        }

        private void SetListSize()
        {
            if (UnitPers.Count > 0 && UnitPers.Count <= 10)
            {
                ListSize = UnitPers.Count; 
            }
            else if (UnitPers.Count > 10)
            {
                ListSize = 10;
            }

            StateHasChanged();
        }

        private void ValidateUnitPer()
        {
            ValidationErrors.Clear();
            UnitPerValidator unitPerValidator = new UnitPerValidator();
            ValidationResult unitPerResults = unitPerValidator.Validate(UnitPer);

            if (unitPerResults.IsValid == false)
            {
                foreach (var failure in unitPerResults.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }

            }
        }
    }
}
