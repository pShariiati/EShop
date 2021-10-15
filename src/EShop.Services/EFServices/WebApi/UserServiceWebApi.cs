using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EShop.Services.Contracts;
using EShop.Services.Contracts.WebApi;
using EShop.ViewModels.TestWebApi;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EShop.Services.EFServices.WebApi
{
    public class UserServiceWebApi : IUserServiceWebApi
    {
        private readonly ICookieManager _cookieManager;
        private readonly IHttpClientService _clientService;

        public UserServiceWebApi(ICookieManager cookieManager, IHttpClientService clientService)
        {
            _cookieManager = cookieManager;
            _clientService = clientService;
        }

        public async Task<OperationResult<string>> Add(AddUserViewModel input)
        {
            var token = _cookieManager.GetValue("JWTToken");
            var modelInJson = JsonConvert.SerializeObject(input);
            var result = await _clientService.SendAsync(
                "https://localhost:5003/user/addbase64",
                HttpMethod.Post,
                token,
                modelInJson
            );
            if ((int)result.StatusCode != StatusCodes.Status201Created)
            {
                return new OperationResult<string>(false, "نام کاربری تکراری است");
            }
            return new OperationResult<string>(true, null);
        }

        public async Task<OperationResult<List<ShowUserViewModel>>> GetAllAsync()
        {
            var token = _cookieManager.GetValue("JWTToken");
            var result = await _clientService.SendAsync(
                "https://localhost:5003/user/index",
                HttpMethod.Get,
                token
            );
            if ((int)result.StatusCode != StatusCodes.Status200OK)
            {
                return new OperationResult<List<ShowUserViewModel>>(false, null);
            }
            var responseBody = await result.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<ShowUserViewModel>>(responseBody);
            return new OperationResult<List<ShowUserViewModel>>(true, users);
        }

        public async Task<OperationResult<string>> Login(LoginViewModel input)
        {
            var modelInJson = JsonConvert.SerializeObject(input);
            var result = await _clientService.SendAsync(
                "https://localhost:5003/account/login",
                HttpMethod.Post,
                content: modelInJson
            );
            if ((int)result.StatusCode != StatusCodes.Status200OK)
            {
                return new OperationResult<string>(false, "نام کاربری یا رمز عبور اشتباه است");
            }
            // token
            var responseBody = await result.Content.ReadAsStringAsync();
            return new OperationResult<string>(true, responseBody);
        }
    }
}
