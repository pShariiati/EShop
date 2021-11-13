using AutoMapper;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.EFServices;

public class CartService : GenericService<Cart>, ICartService
{
    private readonly IUnitOfWork _uow;
    private readonly DbSet<Cart> _carts;
    private readonly IMapper _mapper;

    public CartService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _uow = uow;
        _mapper = mapper;
        _carts = uow.Set<Cart>();
    }

    public Task<Cart> GetUserCartAsync(int userId)
        => _carts
            .Where(x => !x.IsPay)
            .SingleOrDefaultAsync(x => x.UserId == userId);

    public async Task<List<ShowCartPreviewForClientViewModel>> GetUserCartsForClient(int userId)
    {
        var result = await _mapper.ProjectTo<ShowCartPreviewForClientViewModel>(
            _carts.Where(x => x.UserId == userId)
        ).ToListAsync();
        //var result = _carts.Where(x => x.UserId == userId)
        //    .Select(x => new ShowCartPreviewForClientViewModel()
        //    {
        //        Address = x.Address,
        //        Id = x.Id,
        //        IsPay = x.IsPay,
        //        RefId = x.RefId,
        //        TotalPrice = x.TotalPrice
        //    }).ToListAsync();
        return result;
    }

    public Task<List<ShowCartPreviewForAdminViewModel>> GetUserCartsForAdmin()
        => _carts.Where(x => x.IsPay)
            .Select(x => new ShowCartPreviewForAdminViewModel()
            {
                CustomerFullName = x.User.FullName,
                Address = x.Address,
                Id = x.Id,
                IsPay = x.IsPay,
                RefId = x.RefId,
                TotalPrice = x.TotalPrice
            }).ToListAsync();
}
