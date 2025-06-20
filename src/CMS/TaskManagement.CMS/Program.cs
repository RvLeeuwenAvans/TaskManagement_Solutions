using System.Reflection;
using MudBlazor.Services;
using TaskManagement.CMS.Components;

var builder = WebApplication.CreateBuilder(args);

// add configuration file into proj.        
var assembly = Assembly.GetExecutingAssembly();
using var stream = assembly.GetManifestResourceStream("TaskManagement.MobileApp.Properties.appsettings.json");
// I mean; we ignore the possible null. but uh… just make sure to not delete the appsettings.json.
var config = new ConfigurationBuilder()
    .AddJsonStream(stream!)
    .Build();

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
