using Microsoft.AspNetCore.Components;
using WebStore.DTO;

namespace WebStore.WEB.Components
{
    public partial class CustomerComponent
    {
        [Parameter]
        public CustomerDTO Customer { get; set; } = new CustomerDTO();

        [Parameter]
        public List<string> ValidationErrors { get; set; } = new List<string>();
    }
}
