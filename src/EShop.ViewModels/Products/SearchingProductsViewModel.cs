using EShop.ViewModels.Categories;
using System.Collections.Generic;

namespace EShop.ViewModels.Products
{
    public class SearchingProductsViewModel
    {
        public List<ProductCartViewModel> Products { get; set; }
        public ProductSearchConditionEnum Condition { get; set; }
        public List<CategoryAllFields> Categories { get; set; }
        public List<int> SelectedCategories { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int SelectedMinPrice { get; set; }
        public int SelectedMaxPrice { get; set; }
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
    }

    public class ProductCartViewModel
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public int Id { get; set; }
        public int Price { get; set; }
    }

    public class ProductCartsWithPagination
    {
        public List<ProductCartViewModel> Products { get; set; }
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
    }
}