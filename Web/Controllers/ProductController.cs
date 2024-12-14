using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product/Create
        // Controller ProductController

        [HttpGet]
        public IActionResult Create(int storeId)
        {
            // Truyền StoreId vào view model
            var viewModel = new Product
            {
                TheLoaiId = storeId  // Đặt storeId vào thuộc tính của product, hoặc có thể dùng để lấy thông tin cửa hàng
            };

            // Bạn có thể lấy danh sách thể loại hoặc các thông tin khác nếu cần
            ViewBag.StoreId = storeId;
            ViewBag.StoreList = _context.Stores.ToList();  // Ví dụ lấy danh sách cửa hàng
            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Lưu sản phẩm vào bảng Product
                _context.Add(product);
                await _context.SaveChangesAsync();  // Lưu sản phẩm để có ID

                // Tạo mối quan hệ giữa sản phẩm và cửa hàng trong bảng ProductStore
                var productStore = new ProductStore
                {
                    ProductId = product.ID,  // ID của sản phẩm mới
                    StoreId = product.TheLoaiId  // ID của cửa hàng (TheLoaiId dùng để lưu StoreId)
                };

                // Lưu mối quan hệ vào bảng ProductStore
                _context.ProductStores.Add(productStore);
                await _context.SaveChangesAsync();  // Lưu mối quan hệ

                return RedirectToAction(nameof(Index));  // Chuyển hướng đến trang danh sách sản phẩm
            }

            return View(product);
        }





    }
}
