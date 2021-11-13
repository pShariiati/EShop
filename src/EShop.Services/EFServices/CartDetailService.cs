using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.EFServices;

public class CartDetailService : GenericService<CartDetail>, ICartDetailService
{
    private readonly IUnitOfWork _uow;
    private readonly DbSet<CartDetail> _cartDetails;

    public CartDetailService(IUnitOfWork uow)
        : base(uow)
    {
        _uow = uow;
        _cartDetails = uow.Set<CartDetail>();
    }

    public Task<CartDetail> GetCartDetailsBy(int productId, int userId)
        => _cartDetails.Where(x => x.Cart.UserId == userId)
            .Where(x => !x.Cart.IsPay)
            .SingleOrDefaultAsync(x => x.ProductId == productId);

    public Task<int> CalculateUserCartTotalPrice(int userId)
        => _cartDetails.Where(x => !x.Cart.IsPay)
            .Where(x => x.Cart.UserId == userId)
            .SumAsync(x => x.Price * x.Count);

    public Task<List<CartDetailPreviewViewModel>> GetCartDetailsBy(int userId)
        => _cartDetails.Where(x => x.Cart.UserId == userId)
            .Where(x => !x.Cart.IsPay)
            .Select(x => new CartDetailPreviewViewModel()
            {
                ProductId = x.ProductId,
                Count = x.Count,
                Price = x.Price,
                ProductImage = x.Product.ProductImages.First().Title,
                ProductTitle = x.Product.Title
            }).ToListAsync();

    public Task<CartDetail> FindBy(int productId, int userId)
        => _cartDetails.Where(x => x.Cart.UserId == userId)
            .Where(x => !x.Cart.IsPay)
            .SingleOrDefaultAsync(x => x.ProductId == productId);

    public Task<List<CartDetailPreviewViewModel>> GetCartDetails(int userId, int cartId)
        => _cartDetails.Where(x => x.CartId == cartId)
            .Where(x => x.Cart.UserId == userId)
            .Select(x => new CartDetailPreviewViewModel()
            {
                Price = x.Price,
                Count = x.Count,
                ProductId = x.ProductId,
                ProductImage = x.Product.ProductImages.First().Title,
                ProductTitle = x.Product.Title
            }).ToListAsync();

    public Task<List<CartDetailPreviewForAdminViewModel>> GetCartDetailsForAdmin(int cartId)
        => _cartDetails.Where(x => x.CartId == cartId)
            .Where(x => x.Cart.IsPay)
            .Select(x => new CartDetailPreviewForAdminViewModel()
            {
                CustomerFullName = x.Cart.User.FullName,
                Price = x.Price,
                Count = x.Count,
                ProductId = x.ProductId,
                ProductImage = x.Product.ProductImages.First().Title,
                ProductTitle = x.Product.Title
            }).ToListAsync();
}
