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
using EShop.Common.Security;
using EShop.Services.Contracts.WebApi;
using EShop.Services.EFServices.WebApi;
using Ganss.XSS;

namespace EShop.IocConfig
{
    public static class AddCustomServicesExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var connectionStrings = provider.GetRequiredService<IOptionsMonitor<ConnectionStringsModel>>().CurrentValue;
            services.AddDbContext<EShopDbContext>(options =>
            {
                options.UseSqlServer(connectionStrings.EShopDbContextConnection);
            });

            #region RegisterIdentityServices

            services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimService>();
            services.AddScoped<UserClaimsPrincipalFactory<User, Role>, UserClaimService>();

            services.AddScoped<IRoleManagerService, RoleManagerService>();
            services.AddScoped<RoleManager<Role>, RoleManagerService>();

            services.AddScoped<IRoleStoreService, RoleStoreService>();
            services.AddScoped<RoleStore<Role, EShopDbContext, int, UserRole, RoleClaim>, RoleStoreService>();

            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<UserManager<User>, UserManagerService>();

            services.AddScoped<IUserStoreService, UserStoreService>();
            services.AddScoped<UserStore<User, Role, EShopDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>,
                UserStoreService>();

            services.AddScoped<ISignInManagerService, SignInManagerService>();
            services.AddScoped<SignInManager<User>, SignInManagerService>();

            #endregion
            
            services.AddScoped<IUnitOfWork, EShopDbContext>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartDetailService, CartDetailService>();
            services.AddScoped<IProductTagService, ProductTagService>();
            services.AddScoped<ICookieManager, CookieManager>();
            services.AddScoped<IUserServiceWebApi, UserServiceWebApi>();
            services.AddScoped<IHttpClientService, HttpClientService>();
            services.AddScoped<IRijndaelEncryption, RijndaelEncryption>();
            services.AddIdentity<User, Role>(identityOptions =>
                {
                    SetPasswordOptions(identityOptions.Password);

                    identityOptions.Lockout.AllowedForNewUsers = false;
                    identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                    identityOptions.Lockout.MaxFailedAccessAttempts = 3;

                    identityOptions.SignIn.RequireConfirmedEmail = true;
                    identityOptions.SignIn.RequireConfirmedPhoneNumber = false;
                    identityOptions.SignIn.RequireConfirmedAccount = true;

                    identityOptions.User.RequireUniqueEmail = true;
                })
                .AddUserStore<UserStoreService>()
                .AddUserManager<UserManagerService>()
                .AddRoleStore<RoleStoreService>()
                .AddRoleManager<RoleManagerService>()
                .AddSignInManager<SignInManagerService>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                //.AddEntityFrameworkStores<EShopDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "****";
                    options.ClientSecret = "****";
                });
            services.AddRazorViewRenderer();

            #region Html sanitizer
            IHtmlSanitizer sanitizer = new HtmlSanitizer();
            //services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>();
            services.AddSingleton(sanitizer);
            #endregion

            return services;
        }
        public static IServiceCollection AddRazorViewRenderer(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IViewRendererService, ViewRendererService>();
            return services;
        }

        private static void SetPasswordOptions(PasswordOptions passwordOptions)
        {
            passwordOptions.RequireDigit = false;
            passwordOptions.RequireLowercase = false;
            passwordOptions.RequireUppercase = true;
            passwordOptions.RequireNonAlphanumeric = false;
        }
    }
}
