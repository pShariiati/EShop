using EShop.IocConfig;
using EShop.ViewModels.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Ticket.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ConnectionStringsModel>(Configuration.GetSection("ConnectionStrings"));
        services.AddCustomServicesForWebApi();
        services.AddCors(options =>
        {
            options.AddPolicy("CustomCORS",
                builder =>
                {
                    builder.WithOrigins("https://localhost:5001")
                        .WithMethods(HttpMethods.Get, HttpMethods.Post)
                        .AllowAnyHeader();
                });
        });
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ticket.WebApi",
                Version = "v1",
                Contact = new OpenApiContact()
                {
                    Url = new Uri("https://site.com"),
                    Email = "pshariiati@gmail.com",
                    Name = "Payam Shariati"
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT",
                    Url = new Uri("https://site.com/license")
                }
            });

            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
            xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));
        });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(
                c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket.WebApi v1")
            );
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        //app.UseCors("CustomCORS");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
