namespace Web.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string imgUrl { get; set; }
        public string Phuong { get; set; }
        public string Duong { get; set; }
        public string SoNha { get; set; }

        // Mối quan hệ với bảng Product thông qua ProductStore
        public List<ProductStore>? ProductStores { get; set; }
    }
}