namespace Web.Models
{
    public class Product
    {
        public int ID { get; set; }  // ID của điều ước

        public string Name { get; set; }  // Nội dung của điều ước

        public string Description { get; set; }
        public double Price { get; set; }
        public string? imgurl { get; set; }
        public int TheLoaiId { get; set; }


    }
}
