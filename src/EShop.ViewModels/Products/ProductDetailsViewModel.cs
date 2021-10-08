using System.Collections.Generic;

namespace EShop.ViewModels.Products
{
    public class ProductDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public string CategoryTitle { get; set; }

        public List<string> Images { get; set; }

        public List<string> Properties { get; set; }

        public List<string> Tags { get; set; }
    }
}