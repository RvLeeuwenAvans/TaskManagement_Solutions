using System.Reflection;
using TaskManagement.CMS.Components;
using TaskManagement.CMS.Plumbing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// add configuration file into proj.        
var assembly = Assembly.GetExecutingAssembly();
using var stream = assembly.GetManifestResourceStream("TaskManagement.CMS.Properties.appsettings.json");
// I mean; we ignore the possible null. Make sure to not delete the appsettings.json.
var config = new ConfigurationBuilder()
    .AddJsonStream(stream!)
    .Build();
        
builder.Configuration.AddConfiguration(config);

builder.Services.ConfigureDefaultServices(builder.Configuration);

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