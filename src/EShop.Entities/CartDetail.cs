namespace EShop.Entities
{
    public class CartDetail : BaseEntity
    {
        #region Fields

        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }

        #endregion

        #region Relations

        public Product Product { get; set; }
        public Cart Cart { get; set; }

        #endregion
    }
}