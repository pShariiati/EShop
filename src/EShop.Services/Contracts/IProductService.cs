using EShop.Entities;
using EShop.ViewModels.Products;

namespace EShop.Services.Contracts;

public interface IProductService : IGenericService<Product>
{
    Task<EditProductViewModel> GetProductToEdit(int id);
    Task<Product> GetProductToUpdateAsync(int id);
    Task<List<ShowProductViewModel>> GetProductsPreviewAsync();
    Task<ProductDetailsViewModel> GetProductDetails(int productId);
    Task<List<ProductPreviewViewModel>> GetNewestProductAsync(int? excludeId = null, int take = 8);
    Task<List<ProductPreviewViewModel>> GetBestSellingProductAsync(int take = 5);
    Task<List<ShowProductInComboBoxViewModel>> GetProductForComboBox();
    ProductCartsWithPagination GetProductsWithFilterAndPagination(List<int> selectedCategories, int minPrice, int maxPrice, int selectedPage, string searchKey = "", int take = 2, ProductSearchConditionEnum condition = ProductSearchConditionEnum.Newest);
    int GetMinPrice();
    int GetMaxPrice();

    Product GetProductToDelete(int productId);
}
