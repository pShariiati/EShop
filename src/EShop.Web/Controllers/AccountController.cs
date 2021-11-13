using AutoMapper;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.Common.Mvc;
using EShop.Entities.Identity;
using EShop.Services.Contracts;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShop.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserManagerService _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IViewRendererService _viewRendererService;
    private readonly IEmailSenderService _emailSenderService;
    private readonly ISignInManagerService _signInManagerService;
    private readonly IMapper _mapper;

    public AccountController(
        IUserManagerService userManager,
        ILogger<AccountController> logger,
        IViewRendererService viewRendererService,
        IEmailSenderService emailSenderService,
        ISignInManagerService signInManagerService, IMapper mapper)
    {
        _userManager = userManager;
        _logger = logger;
        _viewRendererService = viewRendererService;
        _emailSenderService = emailSenderService;
        _signInManagerService = signInManagerService;
        _mapper = mapper;
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var errors = new List<string>();
        var isAuthenticated = User.Identity.IsAuthenticated;
        if (!isAuthenticated)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                //var user = new User
                //{
                //    UserName = model.UserName,
                //    Email = model.Email,
                //    CreatedDateTime = DateTime.Now,
                //    IsActive = true,
                //    Avatar = PublicConstantStrings.UserDefaultAvatar
                //};
                user.CreatedDateTime = DateTime.Now;
                user.IsActive = true;
                user.Avatar = PublicConstantStrings.UserDefaultAvatar;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation(LogCodes.RegisterCode, $"{user.UserName} creates a new account.");
                    var activationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var body = await _viewRendererService.RenderViewToStringAsync(
                        "~/Views/EmailTemplates/_ActivationUserEmailTemplate.cshtml",
                        new RegisterEmailConfirmationViewModel()
                        {
                            UserName = model.UserName,
                            ActivationCode = activationCode,
                            CreatedDateTime = user.CreatedDateTime.ToString()
                        });
                    await _emailSenderService.SendEmailAsync(
                        model.Email,
                        "فعال سازی حساب کاربری",
                        body
                    );
                    return Json("Success");
                }

                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
            }
            else
                errors.Add(PublicConstantStrings.ModelStateErrorMessage);
        }
        else
            errors.Add("شما قبلا وارد سیستم شده اید");
        return BadRequest(errors);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckUserName(string userName, int? id)
    {
        if (User.Identity.IsAuthenticated && id is not null)
        {
            // کاربر فعلی
            var currentUser = await _userManager.FindByIdAsync(id.ToString());
            if (string.Equals(currentUser.UserName, userName, StringComparison.OrdinalIgnoreCase))
                return Json(true);
        }
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return Json(true);
        return Json(false);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckEmail(string email, int? id)
    {
        if (User.Identity.IsAuthenticated && id is not null)
        {
            // کاربر فعلی
            var currentUser = await _userManager.FindByIdAsync(id.ToString());
            if (string.Equals(currentUser.Email, email, StringComparison.OrdinalIgnoreCase))
                return Json(true);
        }
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Json(true);
        return Json(false);
    }

    public async Task<IActionResult> Login(string returnUrl)
    {
        var model = new LoginViewModel
        {
            ExternalLogins = (await _signInManagerService.GetExternalAuthenticationSchemesAsync()).ToList()
        };
        if (User.Identity.IsAuthenticated)
            return RedirectToAction(nameof(HomeController.Index), "Home");
        ViewData["ReturnUrl"] = returnUrl;
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string returnUrl, LoginViewModel model)
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction(nameof(HomeController.Index), "Home");
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور اشتباه است");
            }
            else if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "ابتدا حساب کاربری خود را فعال کنید");
            }
            else if (!user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "حساب کاربری شما غیر فعال شده است");
            }
            else
            {
                var result = await _signInManagerService.PasswordSignInAsync(
                    user, model.Password, model.RememberMe, false
                );
                if (result.Succeeded)
                {
                    _logger.LogInformation(LogCodes.LoginCode, $"{model.UserName} logged in. => {DateTime.Now}");
                    //var userClaims = await _userManager.GetClaimsAsync(user);
                    //if (userClaims.All(x => x.Type != IdentityClaimNames.FullName))
                    //    await _userManager.AddClaimAsync(user, new Claim(IdentityClaimNames.FullName, string.IsNullOrWhiteSpace(user.FullName) ? user.UserName : user.FullName));
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور اشتباه است");
            }
        }
        else
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> NavbarLogin(LoginViewModel model)
    {
        var errors = new List<string>();
        var isUserAuthenticated = User.Identity.IsAuthenticated;
        if (!isUserAuthenticated)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    errors.Add("نام کاربری یا رمز عبور اشتباه است");
                }
                else if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    errors.Add("ابتدا حساب کاربری خود را فعال کنید");
                }
                else if (!user.IsActive)
                {
                    errors.Add("حساب کاربری شما غیر فعال شده است");
                }
                else
                {
                    var result = await _signInManagerService.PasswordSignInAsync(
                        user, model.Password, model.RememberMe, false
                    );
                    if (result.Succeeded)
                    {
                        //var userClaims = await _userManager.GetClaimsAsync(user);
                        //if (userClaims.All(x => x.Type != IdentityClaimNames.FullName))
                        //    await _userManager.AddClaimAsync(user,
                        //        new Claim(IdentityClaimNames.FullName,
                        //            string.IsNullOrWhiteSpace(user.FullName) ? user.UserName : user.FullName));
                        _logger.LogInformation(LogCodes.LoginCode,
                            $"{model.UserName} logged in. => {DateTime.Now}");
                        return Ok("Success");
                    }

                    errors.Add("نام کاربری یا رمز عبور اشتباه است");
                }
            }
            else
                errors.Add(PublicConstantStrings.ModelStateErrorMessage);
        }
        else
            errors.Add("شما قبلا وارد سیستم شده اید");
        return BadRequest(errors);
    }

    public async Task<IActionResult> ConfirmationAccount(string userName, string code)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(code))
            return View("Error");
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return View("NotFound");
        var result = await _userManager.ConfirmEmailAsync(user, code);
        return View(result.Succeeded ? nameof(ConfirmationAccount) : "Error");
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return View("ForgotPasswordConfirmation");
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            ModelState.AddModelError(string.Empty, "ابتدا حساب کاربری خود را فعال کنید");
            return View();
        }

        var resetPasswordCode = await _userManager.GeneratePasswordResetTokenAsync(user);
        var body = await _viewRendererService.RenderViewToStringAsync(
            "~/Views/EmailTemplates/_ForgotPasswordEmailTemplate.cshtml",
            new ForgotPasswordEmailViewModel()
            {
                UserName = user.UserName,
                ResetPasswordCode = resetPasswordCode
            });
        await _emailSenderService.SendEmailAsync(
            model.Email,
            "بازنشانی رمز عبور",
            body
        );
        return View("ForgotPasswordConfirmation");
    }

    public IActionResult ResetPassword(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return View("Error");
        ViewData["Token"] = code;
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        ViewData["Token"] = model.Token;
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return View("ResetPasswordConfirmation");
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
            return View("ResetPasswordConfirmation");
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
        return View(model);
    }

    public PartialViewResult LoadLoginPartial() => PartialView("_LoginPartial");

    public PartialViewResult LoadRegisterPartial() => PartialView("_RegisterPartial");

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;
        await _signInManagerService.SignOutAsync();
        if (user is not null)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            _logger.LogInformation(LogCodes.LogoutCode, $"{user.UserName} logged out.");
        }
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string returnUrl)
    {
        if (returnUrl == "/Account/ConfirmationAccount")
            returnUrl = string.Empty;
        var redirectUrl =
            Url.Action(nameof(ExternalLoginCallBack), "Account",
            new { ReturnUrl = returnUrl, area = string.Empty });

        var properties = _signInManagerService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        var loginViewModel = new LoginViewModel()
        {
            ExternalLogins = (await _signInManagerService.GetExternalAuthenticationSchemesAsync()).ToList()
        };

        if (remoteError != null)
        {
            ModelState.AddModelError(string.Empty, $"Error : {remoteError}");
            return View(nameof(Login), loginViewModel);
        }

        var externalLoginInfo = await _signInManagerService.GetExternalLoginInfoAsync();
        if (externalLoginInfo == null)
        {
            ModelState.AddModelError(string.Empty, "خطایی به وجود آمد، مجددا تلاش نماید");
            return View(nameof(Login), loginViewModel);
        }

        var signInResult = await _signInManagerService
            .ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey, true, true);

        if (signInResult.Succeeded)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(HomeController.Index),
                "Home", new { area = string.Empty });
        }

        var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

        if (email != null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User()
                {
                    UserName = Guid.NewGuid().ToString("N"),
                    Email = email,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedDateTime = DateTime.Now,
                    Avatar = PublicConstantStrings.UserDefaultAvatar
                    //UserClaims = new List<UserClaim>()
                    //{
                    //    new ()
                    //    {
                    //        ClaimType = IdentityClaimNames.FullName,
                    //        ClaimValue = "- - -"
                    //    }
                    //}
                };
                _logger.LogInformation(LogCodes.RegisterCode, $"{user.UserName} creates a new account.");
                await _userManager.CreateAsync(user);
            }
            _logger.LogInformation(LogCodes.LoginCode, $"{user.UserName} logged in. => {DateTime.Now}");
            await _userManager.AddLoginAsync(user, externalLoginInfo);
            await _signInManagerService.SignInAsync(user, true);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty });
        }
        return View("Error");
    }

    public IActionResult AccessDenied() => View();

    [Authorize]
    public async Task<IActionResult> EditAccount()
    {
        //var userId = int.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        var userId = User.Identity.GetUserId();
        var user = await _userManager.GetUserForEditAccountAsync(userId);
        if (user is null)
            return View("Error");
        return View(user);
    }

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAccount(EditAccountViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return View("Error");
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
            }
            if (model.Avatar != null && model.Avatar.Length > 0)
            {
                var avatarName = Guid.NewGuid().ToString("N");
                var avatarExtension = System.IO.Path.GetExtension(model.Avatar.FileName);
                model.Avatar.SaveImage(avatarName, avatarExtension, "avatars");
                user.Avatar = avatarName + avatarExtension;
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManagerService.RefreshSignInAsync(user).ConfigureAwait(false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        else
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
        return View(model);
    }
}