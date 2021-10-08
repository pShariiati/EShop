using System.ComponentModel.DataAnnotations;

namespace EShop.Entities
{
    public class ProductProperty : BaseEntity
    {
        #region Fields

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; }

        public int ProductId { get; set; }

        #endregion

        #region Relations

        public virtual Product Product { get; set; }

        #endregion
    }
}
