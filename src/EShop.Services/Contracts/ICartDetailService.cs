using EShop.Entities;
using EShop.ViewModels.Cart;

namespace EShop.Services.Contracts;

public interface ICartDetailService : IGenericService<CartDetail>
{
    Task<CartDetail> GetCartDetailsBy(int productId, int userId);
    Task<int> CalculateUserCartTotalPrice(int userId);
    Task<List<CartDetailPreviewViewModel>> GetCartDetailsBy(int userId);
    Task<CartDetail> FindBy(int productId, int userId);
    Task<List<CartDetailPreviewViewModel>> GetCartDetails(int userId, int cartId);
    Task<List<CartDetailPreviewForAdminViewModel>> GetCartDetailsForAdmin(int cartId);
}
