using Microsoft.AspNetCore.Components;
using WebStore.DTO;

namespace WebStore.WEB.Components
{
    public partial class CustomerRegistrationComponent
    {
        [Parameter]
        public UserRegistrationDTO Customer { get; set; } = new UserRegistrationDTO();

        [Parameter]
        public List<string> ValidationErrors { get; set; } = new List<string>();


    }
}
