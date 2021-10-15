using System.Collections.Generic;
using System.Threading.Tasks;
using EShop.ViewModels.TestWebApi;

namespace EShop.Services.Contracts.WebApi
{
    public interface IUserServiceWebApi
    {
        Task<OperationResult<string>> Add(AddUserViewModel input);
        Task<OperationResult<List<ShowUserViewModel>>> GetAllAsync();
        Task<OperationResult<string>> Login(LoginViewModel input);
    }
}