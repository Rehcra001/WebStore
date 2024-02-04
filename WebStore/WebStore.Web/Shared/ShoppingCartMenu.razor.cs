using Microsoft.AspNetCore.Components;
using System.Numerics;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Shared
{
    public partial class ShoppingCartMenu : IDisposable
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        private int TotalQuantity { get; set; }

        protected override void OnInitialized()
        {
            //subcribe
            ShoppingCartService.OnShoppingCartChanged += UpdateTotalQuantity;
        }

        private void UpdateTotalQuantity(int totalQty)
        {
            TotalQuantity = totalQty;
            StateHasChanged();
        }

        public void Dispose()
        {
            //Unsubscribe
            ShoppingCartService.OnShoppingCartChanged -= UpdateTotalQuantity;
        }
    }
}
