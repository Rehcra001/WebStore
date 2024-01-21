using Microsoft.AspNetCore.Components;

namespace WebStore.WEB.Components
{
    public partial class ImageComponent
    {
        [Parameter]
        public string? Label { get; set; } = "Label";

        [Parameter]
        public string Source { get; set; } = string.Empty;

        [Parameter]
        public int Height { get; set; }

        [Parameter]
        public int Width { get; set; }

        [Parameter]
        public string Alt { get; set; } = "";
    }
}
