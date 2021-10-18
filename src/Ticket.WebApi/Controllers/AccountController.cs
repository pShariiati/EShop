using System.Collections.Generic;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Account.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Ticket.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AccountController(IConfiguration config, ITokenService tokenService, IUserService userService)
        {
            _config = config;
            _tokenService = tokenService;
            _userService = userService;
        }

        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Everything is OK and you get the JWT token</response>
        /// <response code="400">Check the model state OR ```UserName``` OR ```Password``` is incorrect</response>
        //[Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Login")]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserBy(model);
                if (user != null)
                {
                    var generatedToken = _tokenService
                        .BuildToken(
                            _config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(),
                            user, model.RememberMe);
                    if (generatedToken != null)
                    {
                        return Ok(generatedToken);
                    }
                    else
                    {
                        return BadRequest("Something wrong");
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet("Admin")]
        public IActionResult Admin()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin2")]
        public IActionResult Admin2()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("Admin3")]
        public IActionResult Admin3()
        {
            var claims = User.Claims;
            return Ok();
        }
    }
}
