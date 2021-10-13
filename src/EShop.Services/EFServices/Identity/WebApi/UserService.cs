using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities.WebApiEntities;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Account.WebApi;
using EShop.ViewModels.Users.WebApi;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.EFServices.Identity.WebApi
{
    public class UserService : GenericService<User>, IUserService
    {
        private readonly DbSet<User> _users;

        public UserService(IUnitOfWork uow)
        : base(uow)
        {
            _users = uow.Set<User>();
        }

        public UserToBuildJwtTokenViewModel GetUserBy(LoginViewModel model)
        {
            var hashedPassword = model.Password.ToHash();
            var user = _users
                       .Include(x => x.Roles)
                       .Where(x => x.UserName == model.UserName)
                       .SingleOrDefault(x => x.Password == hashedPassword);
            if (user is null)
                return null;
            return new UserToBuildJwtTokenViewModel
            {
                Roles = user.Roles.Select(x => x.Title).ToList(),
                Id = user.Id,
                UserName = user.UserName
            };
        }

        public bool IsExistsByUserNameForAdd(string userName)
            => _users.Any(x => x.UserName == userName);

        public bool IsExistsByUserNameForEdit(string userName, int userId)
            => _users
                .Where(x => x.Id != userId)
                .Any(x => x.UserName == userName);

        public User GetUserToEdit(int userId)
            => _users.Include(x => x.Roles)
                .SingleOrDefault(x => x.Id == userId);
    }
}
