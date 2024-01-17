using Microsoft.AspNetCore.Components;
using WebStore.DTO;

namespace WebStore.WEB.Components
{
    public partial class AddressComponent
    {
        [Parameter]
        public int Count { get; set; } = 1;

        [Parameter]
        public AddressDTO Address { get; set; } = new AddressDTO();

        [Parameter]
        public List<string> ValidationErrors { get; set; } = new List<string>();

    }
}
