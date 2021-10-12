using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Account.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Ticket.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        
        //[Route("login")]
        [HttpPost("Login")]
        public IActionResult Login(LoginViewModel model)
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
            return BadRequest();
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
