using EShop.Entities;
using EShop.ViewModels.Sliders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EShop.Services.Contracts
{
    public interface ISliderService : IGenericService<Slider>
    {
        Task<List<ShowSliderViewModel>> GetPreviewAsync();

        Task<EditSliderViewModel> GetForEdit(int id);

        Task<List<ShowSliderInFrontViewModel>> GetSlidersForFront();
    }
}