using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShop.Entities
{
    [Index(nameof(ProductTag.Title), IsUnique = true)]
    public class ProductTag : BaseEntity
    {
        #region Fields

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        #endregion

        #region Relations

        //public ICollection<Product> Products { get; set; }
        public ICollection<ProductProductTag> ProductProductTags { get; set; }

        #endregion
    }
}