using Blazored.LocalStorage;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;
using WebStore.WEB.Validators;

namespace WebStore.WEB.Pages
{
    public partial class OrderPreview
    {
        private const string SHOW = "";
        private const string HIDE = "none";

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        public ICustomerService CustomerService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

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

                //Retrieve addresses
                var returned = await CustomerService.GetAddresLinesAsync();

                if (returned != null || returned.Count() > 0)
                {
                    AddressLines = (List<AddressLineDTO>)returned;
                    //select the first address as ship to
                    DefaultShipAddress = AddressLines[0].AddressId;
                    AddressLineDTO addressLine = AddressLines.First(x => x.AddressId == DefaultShipAddress);
                    addressLine.ShipToSelected = true;
                }            
            }
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

        private void SelectAddress(AddressLineDTO selected)
        {

            foreach (var line in AddressLines)
            {
                
                if (line.AddressId == selected.AddressId)
                {
                    DefaultShipAddress = line.AddressId;
                    line.ShipToSelected = true;
                }
                else
                {
                    line.ShipToSelected = false;
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

        private async Task SaveNewAddress_Click()
        {
            ValidateAddress();

            if (ValidationErrors.Count == 0)
            {
                AddressDTO address = await CustomerService.AddCustomerAddress(NewAddress);
                if (address != null && address.AddressId != default)
                {
                    //Retrieve addresses
                    var returned = await CustomerService.GetAddresLinesAsync();

                    if (returned != null || returned.Count() > 0)
                    {
                        AddressLines = (List<AddressLineDTO>)returned;
                        DefaultShipAddress = AddressLines.Max(x => x.AddressId);
                        AddressLineDTO addressLine = AddressLines.First(x => x.AddressId == DefaultShipAddress);
                        SelectAddress(addressLine);
                    }
                    ShowNewAddressButton = SHOW;
                    ShowNewAddressForm = HIDE;
                }
            }
        }

        private void CancelNewAddress_Click()
        {
            ValidationErrors.Clear();
            ShowNewAddressButton = SHOW;
            ShowNewAddressForm = HIDE;
        }

        private void ProceedToCheckOut_Click()
        {
            NavigationManager.NavigateTo("/orderconfirmation");
        }

        private async void BackToShoppingCart_Click()
        {
            if (await LocalStorageService.ContainKeyAsync("CartItems"))
            {
                await LocalStorageService.RemoveItemAsync("CartItems");
            }
            NavigationManager.NavigateTo("/shoppingcart");

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
