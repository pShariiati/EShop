using AutoMapper;
using EShop.IocConfig;
using EShop.ViewModels.Application;
using EShop.Web.Mappings;
using Microsoft.Extensions.WebEncoders;
using NLog.Web;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});
builder.WebHost.UseNLog();

//builder.Host.ConfigureWebHostDefaults(webBuilder =>
//{
//    webBuilder.ConfigureLogging(
//        logging =>
//        {
//            logging.ClearProviders();
//            logging.SetMinimumLevel(LogLevel.Warning);
//        }).UseNLog();
//});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<ConnectionStringsModel>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<EmailConfigsModel>(builder.Configuration.GetSection("EmailConfigs"));
builder.Services.AddCustomServices();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
builder.Services.Configure<WebEncoderOptions>(options =>
{
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
