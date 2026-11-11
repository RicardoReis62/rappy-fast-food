using System.Globalization;
using RappyFastFood.Web.Components;
using RappyFastFood.Web.Services;

var cultura = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.DefaultThreadCurrentUICulture = cultura;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddScoped<Sessao>();
builder.Services.AddHttpClient<ApiClient>(cliente =>
    cliente.BaseAddress = new Uri(builder.Configuration["ApiUrl"]!));

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
