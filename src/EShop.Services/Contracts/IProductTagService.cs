using EShop.Entities;
using System.Collections.Generic;

namespace EShop.Services.Contracts
{
    public interface IProductTagService : IGenericService<ProductTag>
    {
        List<ProductTag> GetTags(List<string> splittedTags);
    }
}