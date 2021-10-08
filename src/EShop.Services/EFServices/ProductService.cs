using AutoMapper;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Services.EFServices
{
    public class ProductService : GenericService<Product>, IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Product> _products;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork uow, IMapper mapper)
            : base(uow)
        {
            _uow = uow;
            _mapper = mapper;
            _products = uow.Set<Product>();
        }

        public async Task<EditProductViewModel> GetProductToEdit(int id)
        {
            var result = await _mapper.ProjectTo<EditProductViewModel>(
                _products
            ).SingleOrDefaultAsync(x => x.Id == id);
            //var result = _products
            //    .Select(x => new EditProductViewModel()
            //    {
            //        Description = x.Description,
            //        Title = x.Title,
            //        Price = x.Price,
            //        ProductImages = x.ProductImages.Select(i => i.Title).ToList(),
            //        Id = x.Id,
            //        CategoryId = x.Category.ParentId ?? 0,
            //        CategoryChildrenId = x.CategoryId,
            //        Properties = x.ProductProperties.Select(
            //            p => p.Title + " ||| " + p.Value
            //        ).ToList(),
            //        Tags = x.ProductProductTags.Select(t => t.ProductTag.Title).ToList()
            //    }).SingleOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public Task<Product> GetProductToUpdateAsync(int id)
            => _products.Include(x => x.ProductProperties)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductProductTags)
                .SingleOrDefaultAsync(x => x.Id == id);

        public Task<List<ShowProductViewModel>> GetProductsPreviewAsync()
            => _products
                .Select(x => new ShowProductViewModel()
                {
                    CategoryTitle = x.Category.Title,
                    Price = x.Price,
                    Id = x.Id,
                    Title = x.Title,
                    CanRemove = !x.CartDetails.Any()
                }).ToListAsync();

        public Task<ProductDetailsViewModel> GetProductDetails(int productId)
            => _products
                .Select(x => new ProductDetailsViewModel()
                {
                    Properties = x.ProductProperties.Select(p => p.Title + " ||| " + p.Value).ToList(),
                    CategoryTitle = x.Category.Title,
                    Description = x.Description,
                    Id = x.Id,
                    Images = x.ProductImages.Select(i => i.Title).ToList(),
                    Price = x.Price,
                    Title = x.Title,
                    Tags = x.ProductProductTags.Select(t => t.ProductTag.Title).ToList()
                }).SingleOrDefaultAsync(x => x.Id == productId);

        public Task<List<ProductPreviewViewModel>> GetNewestProductAsync(int? excludeId = null, int take = 8)
            => _products
                .OrderByDescending(x => x.Id)
                .Take(take)
                .Where(x => excludeId != null ? x.Id != excludeId : true)
                //.Where(x => excludeId == null || x.Id != excludeId)
                .Select(x => new ProductPreviewViewModel()
                {
                    Id = x.Id,
                    Image = x.ProductImages.First().Title,
                    Title = x.Title
                }).ToListAsync();
        public Task<List<ProductPreviewViewModel>> GetBestSellingProductAsync(int take = 5)
            => _products
                .Where(x => x.CartDetails.Any(c => c.Cart.IsPay))
                .OrderByDescending(x => x.CartDetails.Where(c => c.Cart.IsPay)
                    .Sum(c => c.Count))
                .Take(take)
                .Select(x => new ProductPreviewViewModel()
                {
                    Id = x.Id,
                    Image = x.ProductImages.First().Title,
                    Title = x.Title
                }).ToListAsync();

        public Task<List<ShowProductInComboBoxViewModel>> GetProductForComboBox()
            => _products.Select(x => new ShowProductInComboBoxViewModel()
            {
                Id = x.Id,
                Title = x.Title
            }).ToListAsync();

        public ProductCartsWithPagination GetProductsWithFilterAndPagination(
            List<int> selectedCategories,
            int minPrice,
            int maxPrice,
            int selectedPage,
            string searchKey = "",
            int take = 2,
            ProductSearchConditionEnum condition = ProductSearchConditionEnum.Newest)
        {
            var products = _products.Where(x => x.Price >= minPrice)
                .AsQueryable();
            if (maxPrice > 0)
            {
                products = products.Where(x => x.Price <= maxPrice);
            }

            if (!string.IsNullOrWhiteSpace(searchKey))
                products = products.Where(x => x.Title.Contains(searchKey.Trim()));
            if (selectedCategories.Any())
            {
                products = products.Where(x => selectedCategories.Contains(x.CategoryId));
            }
            switch (condition)
            {
                case ProductSearchConditionEnum.Newest:
                    {
                        products = products.OrderByDescending(x => x.Id);
                        break;
                    }
                case ProductSearchConditionEnum.Oldest:
                    {
                        products = products.OrderBy(x => x.Id);
                        break;
                    }
                case ProductSearchConditionEnum.MostExpensive:
                    {
                        products = products.OrderByDescending(x => x.Price);
                        break;
                    }
                case ProductSearchConditionEnum.Cheapest:
                    {
                        products = products.OrderBy(x => x.Price);
                        break;
                    }
                case ProductSearchConditionEnum.BestSelling:
                    {
                        products = products.Where(x => x.CartDetails.Any(c => c.Cart.IsPay))
                            .OrderByDescending(x => x.CartDetails.Where(c => c.Cart.IsPay)
                                .Sum(c => c.Count));
                        break;
                    }
            }
            var allRecordsCount = products.Count();
            var allPagesCount = (int)
                (Math.Ceiling(
                    (decimal)allRecordsCount / take
                ));
            if (selectedPage < 1)
                selectedPage = 1;
            if (selectedPage > allPagesCount)
                selectedPage = allPagesCount;
            var skip = (selectedPage - 1) * take;
            return new ProductCartsWithPagination()
            {
                PagesCount = allPagesCount,
                CurrentPage = selectedPage,
                Products = products
                    .Skip(skip)
                    .Take(take)
                    .Select(x => new ProductCartViewModel()
                    {
                        Id = x.Id,
                        Image = x.ProductImages.First().Title,
                        Price = x.Price,
                        Title = x.Title,
                    }).ToList()
            };
        }

        public int GetMinPrice()
            => _products.Min(x => x.Price);

        public int GetMaxPrice()
            => _products.Max(x => x.Price);

        public Product GetProductToDelete(int productId)
            => _products.Where(x => !x.CartDetails.Any())
                .Include(x => x.ProductImages)
                .SingleOrDefault(x => x.Id == productId);
    }
}