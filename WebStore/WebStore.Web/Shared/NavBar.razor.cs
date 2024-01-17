using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using WebStore.WEB.Providers;

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

    }
}
