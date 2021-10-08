using System.Collections.Generic;

namespace EShop.ViewModels.Categories
{
    public class CategoryAllFields
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public List<CategoryAllFields> Children { get; set; }
    }
}
