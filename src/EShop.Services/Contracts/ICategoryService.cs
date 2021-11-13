﻿using EShop.Entities;
using EShop.ViewModels.Categories;

namespace EShop.Services.Contracts;

public interface ICategoryService : IGenericService<Category>
{
    Task<List<ShowCategory>> AllMainCategoriesAsync();

    Task<List<ShowCategory>> AllMainCategoriesAsync(int currentCategoryId);

    Task<List<ShowCategory>> GetCategoryChildrenAsync(int mainCatId);

    Task<List<CategoryAllFields>> GetAllFieldsAsync();

    Category GetToDelete(int id);
}
