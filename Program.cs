using ChatSimulador.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<CsvService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

// Servir arquivos estáticos do WhatsApp
var whatsappPath = Path.Combine(builder.Environment.ContentRootPath, "Pages", "WhatsApp", "wwwroot");
if (Directory.Exists(whatsappPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(whatsappPath),
        RequestPath = "/whatsapp-assets"
    });
}

// Servir arquivos estáticos do Instagram (quando existir)
var instagramPath = Path.Combine(builder.Environment.ContentRootPath, "Pages", "Instagram", "wwwroot");
if (Directory.Exists(instagramPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(instagramPath),
        RequestPath = "/instagram-assets"
    });
}

// Servir arquivos estáticos do Uber (quando existir)
var uberPath = Path.Combine(builder.Environment.ContentRootPath, "Pages", "Uber", "wwwroot");
if (Directory.Exists(uberPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uberPath),
        RequestPath = "/uber-assets"
    });
}

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
