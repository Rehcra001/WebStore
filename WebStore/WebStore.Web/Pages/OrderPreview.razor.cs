using Blazored.LocalStorage;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages
{
    public partial class OrderPreview
    {
        private const string SHOW = "";
        private const string HIDE = "none";

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Parameter]
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();

        private List<string> ValidationErrors { get; set; } = new List<string>();

        private List<AddressLineDTO> AddressLines { get; set; } = new List<AddressLineDTO>();

        private AddressDTO NewAddress { get; set; } = new AddressDTO();

        private int DefaultShipAddress { get; set; }

        private string ShowNewAddressButton { get; set; } = SHOW;
        private string ShowNewAddressForm { get; set; } = HIDE;

        private const decimal VAT = 0.15M;
        private const decimal VAT_PERCENTAGE = VAT * 100;

        


        protected override async Task OnInitializedAsync()
        {
            if (await LocalStorageService.ContainKeyAsync("CartItems"))
            {
                CartItems = await LocalStorageService.GetItemAsync<List<CartItemDTO>>("CartItems");
                //clear local storage of cart items
                await LocalStorageService.RemoveItemAsync("CartItems");

                //Retrieve addresses
                LoadDummyData();
                //select the first address as ship to
                AddressLines[DefaultShipAddress].ShipToSelected = true;

            }
        }

        private void LoadDummyData()
        {
            AddressLines.Add(new AddressLineDTO
            {
                AddressId = 1,
                AddressLine1 = "21 Stone Arch Village 1, Sunstone rd"
            });

            AddressLines.Add(new AddressLineDTO
            {
                AddressId = 2,
                AddressLine1 = "19 Queen Alexandra rd"
            });

        }

        private decimal CalcLinePrice(int quantity, decimal price)
        {
            return price * quantity;
        }

        private decimal CalcTotalPrice()
        {
            return CartItems.Sum(x => x.Quantity * x.Price);
        }

        private decimal CalcVAT()
        {
            return CalcTotalPrice() * VAT;
        }

        private decimal CalcTotalWithVAT()
        {
            return CalcTotalPrice() + CalcVAT();
        }

        private int SetNewAddressCount()
        {
            return AddressLines.Count + 1;
        }

        private void SelectAddress()
        {

            foreach (var line in AddressLines)
            {
                line.ShipToSelected = !line.ShipToSelected;
                if (line.ShipToSelected)
                {
                    DefaultShipAddress = AddressLines.FindIndex(x => x.AddressId == line.AddressId);
                }
                //Console.WriteLine(line.AddressId + " : " + line.ShipToSelected + " : " + DefaultShipAddress);
            }
        }

        private void AddNewAddress_click()
        {
            NewAddress = new AddressDTO();

            ShowNewAddressButton = HIDE;
            ShowNewAddressForm = SHOW;
        }

        private void SaveNewAddress()
        {
            ValidateAddress();

            if (ValidationErrors.Count == 0)
            {
                ShowNewAddressButton = SHOW;
                ShowNewAddressForm = HIDE;
            }
            
        }

        private void ValidateAddress()
        {
            ValidationErrors.Clear();
            AddressValidator validation = new AddressValidator();
            ValidationResult validationResult = validation.Validate(NewAddress);

            if (validationResult.IsValid == false)
            {
                foreach (var failure in validationResult.Errors)
                {
                    ValidationErrors.Add($"{failure.PropertyName} {failure.ErrorMessage}");
                }
            }
        }
    }
}
