using System.ComponentModel.DataAnnotations;

namespace EShop.Entities
{
    public class ProductImage : BaseEntity
    {
        #region Fields

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public int ProductId { get; set; }

        #endregion

        #region Relations

        public virtual Product Product { get; set; }

        #endregion
    }
}
