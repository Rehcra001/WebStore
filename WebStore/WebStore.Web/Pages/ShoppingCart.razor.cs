using Microsoft.AspNetCore.Components;
using WebStore.DTO;
using WebStore.WEB.Services.Contracts;

namespace WebStore.WEB.Pages
{
    public partial class ShoppingCart
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        private List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();


        protected override async Task OnInitializedAsync()
        {
            try
            {
                var items = await ShoppingCartService.GetCartItems();

                if (items.Count() > 0)
                {
                    CartItems = items.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }     
    }
}
