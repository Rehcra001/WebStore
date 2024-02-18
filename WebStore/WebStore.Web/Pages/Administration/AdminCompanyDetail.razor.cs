using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages.Administration
{
    public partial class AdminCompanyDetail
    {
        [Inject]
        public ICompanyService CompanyService { get; set; }
        [Parameter]
        public string Heading { get; set; } = string.Empty;

        [Parameter]
        public AddressDTO Address { get; set; } = new AddressDTO();

        [Parameter]
        public List<string> ValidationErrors { get; set; } = new List<string>();

        [Parameter]
        public CompanyEFTDTO CompanyEFT { get; set; } = new CompanyEFTDTO();

        [Parameter]
        public CompanyDetailDTO CompanyDetail { get; set; } = new CompanyDetailDTO();

        private string ActionType { get; set; } = string.Empty;
        private string SaveNewButton { get; set; } = string.Empty;
        private string SaveExistsButton { get; set; } = string.Empty;
        private IBrowserFile Image { get; set; }

        private string ImageDataUrl { get; set; } = string.Empty;

        private byte[] ImageBytes { get; set; }

        private const string NEW_RECORD = "New";
        private const string EXISTING_RECORD = "Exists";


        protected override async Task OnInitializedAsync()
        {
            //Check if company details exist
            //try to retrieve
            CompanyDetail = await CompanyService.GetCompanyDetail();

            if (CompanyDetail == null || CompanyDetail.CompanyId == 0)
            {
                ActionType = NEW_RECORD;
                CompanyDetail = new CompanyDetailDTO();
            }
            else
            {
                ActionType = EXISTING_RECORD;
                Address = CompanyDetail.CompanyAddress;
                CompanyEFT = CompanyDetail.CompanyEFT;
                ImageBytes = CompanyDetail.CompanyLogo;
                ImageDataUrl = ImageDataUrl = "data:image;base64," + Convert.ToBase64String(ImageBytes);
            }

            if (ActionType.Equals(NEW_RECORD))
            {
                SaveNewButton = "";
                SaveExistsButton = "none";
            }
            else
            {
                SaveNewButton = "none";
                SaveExistsButton = "";
            }
        }

        private async Task SaveAll()
        {
            Validate();

            if (ValidationErrors.Count == 0)
            {
                if (ActionType.Equals(NEW_RECORD))
                {
                    //Add company logo
                    CompanyDetail.CompanyLogo = ImageBytes;

                    //Add address to DTO
                    CompanyDetail.CompanyAddress = Address;

                    //Add EFT To DTO
                    CompanyDetail.CompanyEFT = CompanyEFT;

                    //Save
                    CompanyDetail = await CompanyService.AddCompanyDetail(CompanyDetail);
                    StateHasChanged();
                }
                else
                {
                    //save each part
                    await SaveCompanyDetail();
                    await SaveEFT();
                    await SaveAddress();
                }
            }
        }

        private async Task SaveEFT()
        {

        }

        private async Task SaveAddress()
        {

        }

        private async Task SaveCompanyDetail()
        {

        }

        private void Validate()
        {
            ValidationErrors.Clear();
            ValidateCompanyDetails();
            ValidateCompanyAddress();
            ValidateCompanyEFTDetails();
        }

        private void ValidateCompanyDetails()
        {
            CompanyDetailsValidator validationRules = new CompanyDetailsValidator();
            ValidationResult validationResult = validationRules.Validate(CompanyDetail);

            if (validationResult.IsValid == false)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }

        private void ValidateCompanyAddress()
        {
            AddressValidator validationRules = new AddressValidator();
            ValidationResult validationResult = validationRules.Validate(Address);

            if (validationResult.IsValid == false)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }

        private void ValidateCompanyEFTDetails()
        {
            CompanyEFTDetailValidator validationRules = new CompanyEFTDetailValidator();
            ValidationResult validationResult = validationRules.Validate(CompanyEFT);

            if (validationResult.IsValid == false)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }

        private async Task PreviewImage(InputFileChangeEventArgs e)
        {
            ImageDataUrl = string.Empty;
            Image = e.File;
            //Image = await e.File.RequestImageFileAsync("image/png", 200, 200);

            try
            {
                using var file = Image.OpenReadStream();
                long len = file.Length;
                ImageBytes = new byte[len];
                await file.ReadAsync(ImageBytes, 0, (int)file.Length);

                ImageDataUrl = "data:image;base64," + Convert.ToBase64String(ImageBytes);

                CompanyDetail.CompanyLogo = ImageBytes;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
