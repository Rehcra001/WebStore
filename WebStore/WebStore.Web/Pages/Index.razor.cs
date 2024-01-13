using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using WebStore.WEB.Providers;

namespace WebStore.WEB.Pages
{
    public partial class Index
    {
        //[Inject]
        //ILocalStorageService LocalStorageService { get; set; }

        //[Inject]
        //AuthenticationStateProvider AuthenticationStateProvider { get; set; }


        //protected override async Task OnInitializedAsync()
        //{
        //    //if (await LocalStorageService.ContainKeyAsync("bearerToken"))
        //    //{
        //    //    await LocalStorageService.RemoveItemAsync("bearerToken");
        //    //    ((AppAuthenticationStateProvider)AuthenticationStateProvider).SignOut();
        //    //}

        //    //StateHasChanged();
        //}
    }
}
