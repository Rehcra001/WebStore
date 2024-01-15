using Microsoft.AspNetCore.Components;

namespace WebStore.WEB.Components
{
    public partial class InputComponent
    {
        [Parameter]
        public List<string>? ValidationErrors { get; set; } = new List<string>();

        [Parameter]
        public string? PropertyName { get; set; }

        [Parameter]
        public string? Label { get; set; } = "Label";

        [Parameter]
        public string? Value { get; set; } = "Value";

        [Parameter]
        public string InputType { get; set; } = "text";

        [Parameter]
        public string? Class { get; set; }


        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        private async Task OnValueChanged(ChangeEventArgs e)
        {
            await ValueChanged.InvokeAsync(e.Value as string);
        }
    }
}
