using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Sliders;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.EFServices;

public class SliderService : GenericService<Slider>, ISliderService
{
    private readonly IUnitOfWork _uow;
    private readonly DbSet<Slider> _sliders;

    public SliderService(IUnitOfWork uow)
        : base(uow)
    {
        _uow = uow;
        _sliders = uow.Set<Slider>();
    }

    public Task<List<ShowSliderViewModel>> GetPreviewAsync()
        => _sliders.Select(x => new ShowSliderViewModel()
        {
            FirstTitle = x.FirstTitle,
            SecondTitle = x.SecondTitle,
            Id = x.Id,
            ProductTitle = x.Product.Title
        }).ToListAsync();

    public Task<EditSliderViewModel> GetForEdit(int id)
        => _sliders.Select(x => new EditSliderViewModel()
        {
            Id = x.Id,
            FirstTitle = x.FirstTitle,
            SecondTitle = x.SecondTitle,
            ProductId = x.ProductId
        }).SingleOrDefaultAsync(x => x.Id == id);

    public Task<List<ShowSliderInFrontViewModel>> GetSlidersForFront()
        => _sliders.Select(x => new ShowSliderInFrontViewModel()
        {
            ProductId = x.ProductId,
            FirstTitle = x.FirstTitle,
            SecondTitle = x.SecondTitle,
            ProductTitle = x.Product.Title,
            Image = x.Image
        }).ToListAsync();
}
