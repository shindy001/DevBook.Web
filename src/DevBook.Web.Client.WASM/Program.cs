using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using DevBook.WebApiClient.Generated;
using DevBook.Web.Client.WASM.Features.Shared;
using DevBook.Web.Client.WASM;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// register the cookie handler
builder.Services.AddScoped<CookieHandler>();
// set up authorization
builder.Services.AddAuthorizationCore();
// register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// register MudBlazor
builder.Services.AddMudServices();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient<IDevBookWebApiClient, DevBookWebApiClient>(
	opt => opt.BaseAddress = new Uri("https://localhost:7126")) // use direct address, .net Aspire discovery does not seem to work for WASM
	.AddHttpMessageHandler<CookieHandler>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
