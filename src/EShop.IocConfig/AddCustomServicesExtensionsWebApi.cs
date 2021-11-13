using EShop.DataLayer.Context;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.Services.EFServices.Identity;
using EShop.Services.EFServices.Identity.WebApi;
using EShop.ViewModels.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EShop.IocConfig;

public static class AddCustomServicesExtensionsWebApi
{
    public static IServiceCollection AddCustomServicesForWebApi(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var connectionStrings = provider.GetRequiredService<IOptionsMonitor<ConnectionStringsModel>>().CurrentValue;
        services.AddDbContext<TicketDbContext>(options =>
        {
            options.UseSqlServer(connectionStrings.TicketDbContextConnection);
        });
        services.AddScoped<IUnitOfWork, TicketDbContext>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRoleService, RoleService>();
        return services;
    }
}
