using EShop.Entities;

namespace EShop.Services.Contracts;

public interface IProductImageService : IGenericService<ProductImage>
{
    Task RemoveProductImageByNameAsync(string productImageName);
}
