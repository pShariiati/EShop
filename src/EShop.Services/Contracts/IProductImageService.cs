using EShop.Entities;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface IProductImageService : IGenericService<ProductImage>
    {
        Task RemoveProductImageByNameAsync(string productImageName);
    }
}