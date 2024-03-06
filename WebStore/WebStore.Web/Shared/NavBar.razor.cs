using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using WebStore.WEB.Providers;
using Microsoft.JSInterop;

namespace WebStore.WEB.Shared
{
    public partial class NavBar
    {
        [Inject]
        ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private bool IsShowMenu { get; set; } = false;

        private int ViewportWidth { get; set; }
        private int ViewportHeight { get; set; }

        private async Task SignOut()
        {
            if (await LocalStorageService.ContainKeyAsync("bearerToken"))
            {
                await LocalStorageService.RemoveItemAsync("bearerToken");
                ((AppAuthenticationStateProvider)AuthenticationStateProvider).SignOut();
            }
            StateHasChanged();
            NavigationManager.NavigateTo("/");
        }

        

        [JSInvokable]
        public void OnResize(int width, int height)
        {
            if (ViewportWidth == width && ViewportHeight == height) return;
            ViewportWidth = width;
            ViewportHeight = height;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("window.registerViewportChangeCallback", DotNetObjectReference.Create(this));
            }
        }

        private void ShowMenu_Click()
        {
            IsShowMenu = !IsShowMenu;
        }

    }
}
