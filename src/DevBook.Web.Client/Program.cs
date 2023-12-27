using DevBook.Web.Client.Features.Shared;
using DevBook.Web.Shared.Extensions;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	// Beware of SSR - after SignalR initialization, exception are handled by Blazor Error Boundaries and this handler will not be called
	app.UseExceptionHandler("/Error", createScopeForErrors: true); 

	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseStatusCodePagesWithRedirects("/404");

app.Run();
