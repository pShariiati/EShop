using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EShop.Services.EFServices
{
    public class ProductTagService : GenericService<ProductTag>, IProductTagService
    {
        private readonly DbSet<ProductTag> _productTags;
        public ProductTagService(IUnitOfWork uow) : base(uow)
        {
            _productTags = uow.Set<ProductTag>();
        }

        public List<ProductTag> GetTags(List<string> splittedTags)
            => _productTags.Where(x => splittedTags.Contains(x.Title)).ToList();
    }
}