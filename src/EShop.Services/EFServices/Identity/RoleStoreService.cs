using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EShop.Services.EFServices.Identity
{
    public class RoleStoreService
    : RoleStore<Role, EShopDbContext, int, UserRole, RoleClaim>,
        IRoleStoreService
    {
        public RoleStoreService(
            IUnitOfWork uow, IdentityErrorDescriber describer = null
            )
        : base((EShopDbContext)uow, describer)
        {

        }
    }
}
