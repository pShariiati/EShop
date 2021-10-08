using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EShop.Services.EFServices
{
    public class ProductImageService : GenericService<ProductImage>, IProductImageService
    {
        private readonly DbSet<ProductImage> _productImages;
        public ProductImageService(IUnitOfWork uow)
            : base(uow)
        {
            _productImages = uow.Set<ProductImage>();
        }

        public async Task RemoveProductImageByNameAsync(string productImageName)
        {
            var productImage = await _productImages
                .SingleOrDefaultAsync(x => x.Title == productImageName);
            if (productImage is not null)
                this.Remove(productImage);
        }
    }
}