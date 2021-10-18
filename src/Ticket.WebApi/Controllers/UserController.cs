using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities.WebApiEntities;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Users.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Ticket.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    //[EnableCors("CustomCORS")]
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
        public List<string> TestData()
        {
            return new List<string>()
            {
                "Payam Shariati",
                "Ali Mohammadi",
                "Sina Rezaei"
            };
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
        public async Task<IActionResult> Add([FromForm] AddUserViewModel model)
        {
            if (ModelState.IsValid)
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
                //upload image
                var avatarName = Guid.NewGuid().ToString("N");
                var avatarExtension = System.IO.Path.GetExtension(model.Avatar.FileName);
                model.Avatar.SaveImage(avatarName, avatarExtension, "avatars");
                user.Avatar = avatarName + avatarExtension;
                //
                await _userService.AddAsync(user);
                await _uow.SaveChangesAsync();
                return CreatedAtAction(nameof(Index), new { id = user.Id }, model);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> AddBase64(AddUserViewModelBase64 model)
        {
            if (ModelState.IsValid)
            {
                var checkForDuplicateUserName = _userService.IsExistsByUserNameForAdd(model.UserName);
                if (checkForDuplicateUserName)
                    return BadRequest(new
                    {
                        Code = 10,
                        Message = "نام کاربری تکراری میباشد"
                    });
                var user = new User()
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Password = model.Password.ToHash()
                };
                if (model.Roles != null && model.Roles.Any())
                {
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
                }

                //upload image
                var avatarName = Guid.NewGuid().ToString("N");
                var avatarExtension = await model.Avatar.SaveBase64ImageAsync(avatarName, "avatars");
                user.Avatar = avatarName + avatarExtension;
                //
                await _userService.AddAsync(user);
                await _uow.SaveChangesAsync();
                return CreatedAtAction(nameof(Index), new { id = user.Id }, model);
            }

            return BadRequest(ModelState);
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
