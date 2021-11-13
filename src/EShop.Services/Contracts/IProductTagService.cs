using EShop.Entities;

namespace EShop.Services.Contracts;

public interface IProductTagService : IGenericService<ProductTag>
{
    List<ProductTag> GetTags(List<string> splittedTags);
}
