using System.Linq;
using System.Threading.Tasks;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities.WebApiEntities;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Account.WebApi;
using EShop.ViewModels.Users.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Ticket.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUnitOfWork _uow;

        public UserController(IConfiguration config, IUserService userService, IUnitOfWork uow, IRoleService roleService)
        {
            _config = config;
            _userService = userService;
            _uow = uow;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var user = await _userService.FindByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            var checkForDuplicateUserName = _userService.IsExistsByUserNameForAdd(model.UserName);
            if (checkForDuplicateUserName)
                return BadRequest("نام کاربری تکراری میباشد");
            var user = new User()
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Password = model.Password.ToHash()
            };
            var existRoles = _roleService.GetRolesBy(model.Roles);
            foreach (var role in model.Roles)
            {
                var currentRole = existRoles.SingleOrDefault(x => x.Title == role);
                //user.Roles.Add(currentRole ?? new Role
                //{
                //    Title = role
                //});
                if (currentRole is null)
                    user.Roles.Add(new Role()
                    {
                        Title = role
                    });
                else
                    user.Roles.Add(currentRole);
            }
            await _userService.AddAsync(user);
            await _uow.SaveChangesAsync();
            return CreatedAtAction(nameof(Index), new { id = user.Id }, model);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = _userService.GetUserToEdit(model.Id);
            if (user is null)
                return BadRequest();
            var checkForDuplicateUserName = _userService.IsExistsByUserNameForEdit(model.UserName, model.Id);
            if (checkForDuplicateUserName)
                return BadRequest("نام کاربری تکراری میباشد");
            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.Password = model.Password.ToHash();
            user.Roles.Clear();
            var existRoles = _roleService.GetRolesBy(model.Roles);
            foreach (var role in model.Roles)
            {
                var currentRole = existRoles.SingleOrDefault(x => x.Title == role);
                //user.Roles.Add(currentRole ?? new Role
                //{
                //    Title = role
                //});
                if (currentRole is null)
                    user.Roles.Add(new Role()
                    {
                        Title = role
                    });
                else
                    user.Roles.Add(currentRole);
            }
            _userService.Update(user);
            await _uow.SaveChangesAsync();
            return CreatedAtAction(nameof(Index), new { id = model.Id }, model);
        }

        //[HttpDelete]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var user = await _userService.FindByIdAsync(userId);
            if (user is null)
                return BadRequest();
            _userService.Remove(user);
            await _uow.SaveChangesAsync();
            return Ok();
        }
    }
}
