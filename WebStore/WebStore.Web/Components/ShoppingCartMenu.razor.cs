using Microsoft.AspNetCore.Components;

namespace WebStore.WEB.Components
{
    public partial class ShoppingCartMenu
    {
        [Parameter]
        public int CartQuantity { get; set; }
    }
}
