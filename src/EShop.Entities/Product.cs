using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Entities;

public class Product : BaseEntity
{
    #region Fields
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [Required]
    [Column(TypeName = "ntext")]
    public string Description { get; set; }

    public int Price { get; set; }

    public int CategoryId { get; set; }
    #endregion

    #region Relations

    public virtual Category Category { get; set; }

    public ICollection<ProductImage> ProductImages { get; set; }
        = new List<ProductImage>();

    public ICollection<ProductProperty> ProductProperties { get; set; }
        = new List<ProductProperty>();

    public ICollection<CartDetail> CartDetails { get; set; }
        = new List<CartDetail>();

    //public ICollection<ProductTag> ProductTags { get; set; }
    //    = new List<ProductTag>();

    public ICollection<ProductProductTag> ProductProductTags { get; set; }
        = new List<ProductProductTag>();

    #endregion
}
