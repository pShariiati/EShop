using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.Common.Security;
using EShop.Services.Contracts;
using EShop.Services.Contracts.WebApi;
using EShop.ViewModels.TestWebApi;
using EShop.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EShop.Web.Controllers;

[TypeFilter(typeof(CustomAuthorize))]
public class WebApiController : Controller
{
    private readonly ICookieManager _cookieManager;
    private readonly IUserServiceWebApi _userService;
    private readonly IRijndaelEncryption _rijndaelEncryption;

    public WebApiController(ICookieManager cookieManager, IUserServiceWebApi userService, IRijndaelEncryption rijndaelEncryption)
    {
        _cookieManager = cookieManager;
        _userService = userService;
        _rijndaelEncryption = rijndaelEncryption;
    }

    public IActionResult GetDataWithAjax()
    {
        return View();
    }

    public IActionResult Login2()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Login2(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Error");
        }
        var builder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "TicketDb",
            IntegratedSecurity = true,
            MultipleActiveResultSets = true
        };
        //var commandText =
        //    $"SELECT TOP(1) * FROM Users WHERE [UserName] = N'{model.UserName}'" +
        //    $" AND [Password] = N'{@model.Password}'";
        var commandText2 =
            $"SELECT TOP(1) * FROM Users WHERE [UserName] = @UserName" +
            $" AND [Password] = @Password";
        using (var connection = new SqlConnection(builder.ConnectionString))
        using (var command = new SqlCommand(commandText2, connection))
        {
            connection.Open();
            command.Parameters.Add(new SqlParameter("@UserName", model.UserName));
            command.Parameters.Add(new SqlParameter("@Password", model.Password));
            var results = command.ExecuteReader();
            if (results.Read())
            {
                var avatar = results["Avatar"];
            }
        }

        return View();
    }

    public async Task<IActionResult> Index()
    {
        //Request.Cookies.TryGetValue("JWTToken", out var token);
        //if (string.IsNullOrWhiteSpace(token))
        //{
        //    return RedirectToAction(nameof(Login));
        //}

        //using var client = new HttpClient();
        //client.DefaultRequestHeaders.Authorization =
        //    new AuthenticationHeaderValue("Bearer", token);
        //var request = new HttpRequestMessage
        //{
        //    Method = HttpMethod.Get,
        //    RequestUri = new Uri("https://localhost:5003/user/index"),
        //    Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json),
        //};

        //var response = await client.SendAsync(request);
        //if ((int)response.StatusCode != StatusCodes.Status200OK)
        //{
        //    return View("Error");
        //}
        //var responseBody = await response.Content.ReadAsStringAsync();
        //var users = JsonConvert.DeserializeObject<List<ShowUserViewModel>>(responseBody);
        var result = await _userService.GetAllAsync();
        if (!result.IsSuccess)
            return View("Error");
        var users = result.Result;
        return View(users);
    }

    public IActionResult Add()
    {
        //Request.Cookies.TryGetValue("JWTToken", out var token);
        //if (string.IsNullOrWhiteSpace(token))
        //{
        //    return RedirectToAction(nameof(Login));
        //}
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    //[TypeFilter(typeof(CustomAuthorize))]
    public async Task<IActionResult> Add(AddUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return View(model);
        }

        //Request.Cookies.TryGetValue("JWTToken", out var token);
        //if (string.IsNullOrWhiteSpace(token))
        //{
        //    return RedirectToAction(nameof(Login));
        //}
        //using var client = new HttpClient();
        //client.DefaultRequestHeaders.Authorization =
        //    new AuthenticationHeaderValue("Bearer", token);
        //await using (var ms = new MemoryStream())
        //{
        //    await model.UserAvatar.CopyToAsync(ms);
        //    var fileBytes = ms.ToArray();
        //    model.Avatar = Convert.ToBase64String(fileBytes);
        //    model.UserAvatar = null;
        //}
        //var modelInJson = JsonConvert.SerializeObject(model);
        //var request = new HttpRequestMessage
        //{
        //    Method = HttpMethod.Post,
        //    RequestUri = new Uri("https://localhost:5003/user/AddBase64"),
        //    Content = new StringContent(modelInJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        //};
        //var result = await client.SendAsync(request);
        //if ((int)result.StatusCode != StatusCodes.Status201Created)
        //{
        //    ModelState.AddModelError(string.Empty, "نام کاربری تکراری است");
        //    return View(model);
        //}
        model.Avatar = await model.UserAvatar.ConvertToBase64();
        model.UserAvatar = null;
        var result = await _userService.Add(model);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Result);
            return View(model);
        }
        return RedirectToAction(nameof(Index));
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                result = false,
                message = PublicConstantStrings.ModelStateErrorMessage
            });
        }

        //var modelInJson = JsonConvert.SerializeObject(model);
        //using var client = new HttpClient();
        //var request = new HttpRequestMessage
        //{
        //    Method = HttpMethod.Post,
        //    RequestUri = new Uri("https://localhost:5003/account/login"),
        //    Content = new StringContent(modelInJson, Encoding.UTF8, MediaTypeNames.Application.Json),
        //};
        //var result = await client.SendAsync(request);
        var result = await _userService.Login(model);
        if (!result.IsSuccess)
        {
            return Json(new
            {
                result = false,
                message = "نام کاربری یا رمز عبور اشتباه است"
            });
        }
        //var resultContent = await result.Content.ReadAsStringAsync();
        var encryptedToken = _rijndaelEncryption.Encryption(result.Result.Trim('"'));
        _cookieManager.Add("JWTToken", encryptedToken, new CookieOptions()
        {
            Expires = model.RememberMe ? DateTimeOffset.Now.AddDays(14) : null,
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        });
        //Response.Cookies.Append("JWTToken", resultContent.Trim('"'));
        return Json(new
        {
            result = true
        });
    }
}

public class CodesMessage
{
    public const int DuplicateUserName = 10;

    public string GetMessage(int code)
    {
        var result = string.Empty;
        if (code == 10)
        {
            result = "نام کاربری تکراری است";
        }
        else if (code == 11)
        {

        }

        return result;
    }
}
