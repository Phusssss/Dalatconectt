namespace Web.Models
{
    public class ProductSearchViewModel
    {
        public List<Product> Products { get; set; }
        public List<Store> Stores { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string PriceSort { get; set; }  // Thêm thuộc tính này để lưu giá trị sắp xếp
    }


}
