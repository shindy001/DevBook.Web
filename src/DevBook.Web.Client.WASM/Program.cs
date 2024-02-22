using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using DevBook.WebApiClient.Generated;
using DevBook.Web.Client.WASM.Features.Shared;
using DevBook.Web.Client.WASM.Identity;
using DevBook.Web.Shared.Extensions;
using DevBook.Web.Client.WASM.ApiClient;
using MudBlazor;
using Blazored.LocalStorage;

Uri DevBookWebApiUri = new("https://localhost:7126");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// set up authorization
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// register command/queries support
builder.Services.AddCommandsAndQueriesExecutor(typeof(Program).Assembly);

// register MudBlazor
builder.Services.AddMudServices(config =>
{
	config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;

	config.SnackbarConfiguration.PreventDuplicates = false;
	config.SnackbarConfiguration.NewestOnTop = false;
	config.SnackbarConfiguration.ShowCloseIcon = true;
	config.SnackbarConfiguration.VisibleStateDuration = 5000;
	config.SnackbarConfiguration.HideTransitionDuration = 500;
	config.SnackbarConfiguration.ShowTransitionDuration = 500;
	config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

// blazored local storage service
builder.Services.AddBlazoredLocalStorage();

// DevBookWebApiFactory creates defautl api client with no delegating handler
builder.Services.AddHttpClient<IDevBookWebApiClientFactory, DevBookWebApiClientFactory>(opt => opt.BaseAddress = DevBookWebApiUri);
builder.Services.AddScoped<ITokenService, TokenService>();

// DevBookWebApi with delegating handler that also handles auto token refresh
builder.Services.AddHttpClient<IDevBookWebApiClient, DevBookWebApiClient>(opt => opt.BaseAddress = DevBookWebApiUri)
	.AddHttpMessageHandler<TokenDelegatingHandler>();
builder.Services.AddScoped<TokenDelegatingHandler>();
builder.Services.AddScoped<IDevBookWebApiActionExecutor, DevBookWebApiActionExecutor>();

await builder.Build().RunAsync();
