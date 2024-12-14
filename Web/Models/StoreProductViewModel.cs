namespace Web.Models
{
    public class StoreProductViewModel
    {
        public Store Store { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

}
