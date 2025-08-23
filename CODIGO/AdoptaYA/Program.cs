using Radzen;
using AdoptaYA.Components;
using AdoptaYA.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddRadzenComponents();

builder.Services.AddConfigureClient(config);    // ConfigExtension
builder.Services.AddServices();                 // ServiceExtension

var app = builder.Build();

// Autenticación en el pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthRoutes();                            // AuthExtension

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
