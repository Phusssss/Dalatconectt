namespace Web.Models
{
    public class ProductStore
    {
      
        public int ProductId { get; set; }
        public int StoreId { get; set; }

        // Điều này tạo mối quan hệ giữa Product và Store
        public Product Product { get; set; }
        public Store Store { get; set; }
    }
}