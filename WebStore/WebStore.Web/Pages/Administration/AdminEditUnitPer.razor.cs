using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages.Administration
{
    public partial class AdminEditUnitPer
    {


        [Inject]
        public IProductService ProductService { get; set; }
        private UnitPerDTO UnitPer { get; set; } = new UnitPerDTO();
        private List<string> ValidationErrors { get; set; } = new List<string>();
        private List<UnitPerDTO> UnitPers { get; set; } = new List<UnitPerDTO>();
        private int ListSize { get; set; } = 1;

        private int unitPerId;
        private int UnitPerId { get => unitPerId;
            set
            {
                unitPerId = value; GetUnitPer(value);
            }
        }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                var unitPers = await ProductService.GetUnitPersAsync();
                if (unitPers.Count() > 0)
                {
                    UnitPers = (List<UnitPerDTO>)unitPers;
                    UnitPers.Sort((x, y) => x.UnitPer.CompareTo(y.UnitPer));
                    SetListSize();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task SaveUnitPer()
        {
            //valdiate
            ValidateUnitPer();

            if (ValidationErrors.Count == 0)
            {
                int count = UnitPers.Count(x => x.UnitPer.ToLower().Equals(UnitPer.UnitPer.ToLower()));
                if (count == 1)
                {
                    //save                

                    UnitPer = await ProductService.UpdateUnitPerAsync(UnitPer);

                    if (UnitPer == null || UnitPer.UnitPerId == 0)
                    {
                        throw new Exception("Unable to save the Unit Per");
                    }
                    UnitPers.Sort((x, y) => x.UnitPer.CompareTo(y.UnitPer));

                    ResetSelection();

                    SetListSize();
                    
                    StateHasChanged();
                }
                else
                {
                    ValidationErrors.Clear();
                    ValidationErrors.Add(nameof(UnitPer.UnitPer) + $" This unit per already exists");
                }
            }

        }

        /// <summary>
        /// reset the InputSelect once saved so that the same selection 
        /// can be edited without selecting off and on again
        /// </summary>
        private void ResetSelection()
        {
            UnitPerId = UnitPerId;
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


        private void GetUnitPer(int? value)
        {
            UnitPer = UnitPers.First(x => x.UnitPerId == value);
        }
    }
}
