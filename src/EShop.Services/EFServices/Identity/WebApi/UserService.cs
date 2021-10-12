using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.DataLayer.Context;
using EShop.Entities.WebApiEntities;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Account.WebApi;
using EShop.ViewModels.Users.WebApi;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.EFServices.Identity.WebApi
{
    public class UserService : IUserService
    {
        private readonly DbSet<User> _users;

        public UserService(IUnitOfWork uow)
        {
            _users = uow.Set<User>();
        }

        public UserToBuildJwtTokenViewModel GetUserBy(LoginViewModel model)
        {
            var user = _users
                       .Include(x => x.Roles)
                       .Where(x => x.UserName == model.UserName)
                       .SingleOrDefault(x => x.Password == model.Password);
            if (user is null)
                return null;
            return new UserToBuildJwtTokenViewModel
            {
                Roles = user.Roles.Select(x=>x.Title).ToList(),
                Id = user.Id,
                UserName = user.UserName
            };
        }
    }
}
