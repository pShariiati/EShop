using System.ComponentModel.DataAnnotations;

namespace EShop.Entities.WebApiEntities
{
    public class Test : BaseEntity
    {
        [Required]
        public string Title { get; set; }
    }
}