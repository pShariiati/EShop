using EShop.Common.Mvc;
using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services;
using EShop.Services.Contracts;
using EShop.Services.Contracts.Identity;
using EShop.Services.EFServices;
using EShop.Services.EFServices.Identity;
using EShop.ViewModels.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace EShop.IocConfig
{
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
            return services;
        }
    }
}
