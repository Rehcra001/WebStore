using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebStore.WEB;
using WebStore.WEB.Providers;
using WebStore.WEB.Services;
using WebStore.WEB.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AppAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(
    provider => provider.GetRequiredService<AppAuthenticationStateProvider>());

//Registration
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

await builder.Build().RunAsync();
