using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using DevBook.WebApiClient.Generated;
using DevBook.Web.Client.WASM.Features.Shared;
using DevBook.Web.Client.WASM.Identity;
using DevBook.Web.Shared.Extensions;
using DevBook.Web.Client.WASM.ApiClient;
using DevBook.Web.Client.WASM.Features.TimeTracking.Tasks;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// set up authorization
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped(sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// register command/queries support
builder.Services.AddCommandsAndQueriesExecutor(typeof(Program).Assembly);

// register MudBlazor
builder.Services.AddMudServices();

// DevBookWebApi client
builder.Services.AddScoped<CookieHandler>();
builder.Services.AddHttpClient<IDevBookWebApiClient, DevBookWebApiClient>(
	opt => opt.BaseAddress = new Uri("https://localhost:7126")) // use direct address, .net Aspire(AppHost proj) discovery does not seem to work for WASM
	.AddHttpMessageHandler<CookieHandler>();
builder.Services.AddScoped<IDevBookWebApiActionExecutor, DevBookWebApiActionExecutor>();

// TimeTracking feature
builder.Services.AddScoped<ITasksService, TasksService>();

await builder.Build().RunAsync();
