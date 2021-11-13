using AutoMapper;
using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Account;
using EShop.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EShop.Services.EFServices.Identity;

public class UserManagerService
: UserManager<User>,
    IUserManagerService
{
    private readonly DbSet<User> _users;
    private readonly IMapper _mapper;

    public UserManagerService(
        IUserStoreService store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManagerService> logger,
        IUnitOfWork uow, IMapper mapper)
    : base(
        (UserStore<User, Role, EShopDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>)store,
        optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _mapper = mapper;
        _users = uow.Set<User>();
    }

    public ShowUsersWithPagination UsersWithPaginationPreview(int selectedPage = 1, int take = 2)
    {
        var users = _users;
        var allRecordsCount = users.Count();
        var allPagesCount = (int)
            (Math.Ceiling(
                (decimal)allRecordsCount / take
            ));
        if (selectedPage < 1)
            selectedPage = 1;
        if (selectedPage > allPagesCount)
            selectedPage = allPagesCount;
        var skip = (selectedPage - 1) * take;
        var mappedUsers = _mapper.ProjectTo<ShowUserViewModel>(
            _users
                .Skip(skip)
                .Take(take)
        ).ToList();
        return new ShowUsersWithPagination()
        {
            CurrentPage = selectedPage,
            PagesCount = allPagesCount,
            //Users = users
            //    .Skip(skip)
            //    .Take(take)
            //    .Select(x => new ShowUserViewModel()
            //{
            //    CreatedDateTime = x.CreatedDateTime,
            //    FullName = x.FullName,
            //    Id = x.Id,
            //    IsActive = x.IsActive,
            //    UserName = x.UserName
            //}).ToList()
            Users = mappedUsers
        };
    }

    public async Task<EditUserViewModel> GetUserForEditAsync(int id)
    {
        var user = await _users.FindAsync(id);
        if (user is null)
            return null;
        var userRoles = await GetRolesAsync(user);
        return new EditUserViewModel
        {
            Email = user.Email,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id,
            SelectedRoles = userRoles.ToList()
        };
    }

    public Task<EditAccountViewModel> GetUserForEditAccountAsync(int id)
        => _users.Select(x => new EditAccountViewModel()
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            UserName = x.UserName
        }).SingleOrDefaultAsync(x => x.Id == id);
}
