using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.EFServices;

public class CategoryService : GenericService<Category>, ICategoryService
{
    private readonly IUnitOfWork _uow;
    private readonly DbSet<Category> _categories;

    public CategoryService(IUnitOfWork uow)
        : base(uow)
    {
        _uow = uow;
        _categories = uow.Set<Category>();
    }

    public Task<List<ShowCategory>> AllMainCategoriesAsync()
        => _categories
            .Where(category => category.ParentId == null)
            .Select(category => new ShowCategory()
            {
                Id = category.Id,
                Title = category.Title,
                CanRemove = !category.Children.Any()
            }).ToListAsync();

    public Task<List<ShowCategory>> AllMainCategoriesAsync(int currentCategoryId)
        => _categories
            .Where(category => category.ParentId == null)
            .Where(category => category.Id != currentCategoryId)
            .Select(category => new ShowCategory()
            {
                Id = category.Id,
                Title = category.Title,
                CanRemove = category.Children.Any()
            }).ToListAsync();

    public Task<List<ShowCategory>> GetCategoryChildrenAsync(int mainCatId)
        => _categories
            .Where(category => category.ParentId == mainCatId)
            .Select(category => new ShowCategory()
            {
                Id = category.Id,
                Title = category.Title,
                CanRemove = !category.Products.Any()
            }).ToListAsync();

    public Task<List<CategoryAllFields>> GetAllFieldsAsync()
        => _categories.Select(x => new CategoryAllFields()
        {
            Children = x.Children.Select(c => new CategoryAllFields()
            {
                Id = c.Id,
                Title = c.Title
            }).ToList(),
            Id = x.Id,
            ParentId = x.ParentId,
            Title = x.Title
        }).ToListAsync();

    public Category GetToDelete(int id)
        => _categories.Where(x => !x.Products.Any())
            .SingleOrDefault(x => x.Id == id);
}
