using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

public class StoreController : Controller
{
    private readonly ApplicationDbContext _context;

    public StoreController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Hiển thị danh sách các cửa hàng
    public IActionResult Index()
    {
        var stores = _context.Stores.ToList(); // Lấy tất cả cửa hàng
        return View(stores);
    }

    // Hiển thị chi tiết cửa hàng và các sản phẩm của nó
    public IActionResult Details(int id)
    {
        var store = _context.Stores
                            .Include(s => s.ProductStores)
                            .ThenInclude(ps => ps.Product)
                            .FirstOrDefault(s => s.Id == id);

        if (store == null)
        {
            return NotFound();
        }

        var model = new StoreProductViewModel
        {
            Store = store,
            Products = store.ProductStores.Select(ps => ps.Product) // Lấy danh sách sản phẩm từ ProductStores
        };

        return View(model);
    }
  

    // Màn hình thêm sản phẩm
    public IActionResult AddProduct(int storeId)
    {
        // Lấy danh sách thể loại và truyền trực tiếp vào ViewBag hoặc ViewData
        ViewData["Categories"] = _context.TheLoais.ToList();

        var model = new ProductStoreViewModel
        {
            StoreId = storeId
        };

        return View(model);
    }
    public IActionResult AddStore()
    {
        return View();
    }

    // Xử lý thêm cửa hàng (POST)
    [HttpPost]
    public IActionResult AddStore(Store model)
    {
        if (ModelState.IsValid)
        {
            // Thêm cửa hàng vào cơ sở dữ liệu
            _context.Stores.Add(model);
            _context.SaveChanges();

            // Chuyển hướng về trang danh sách cửa hàng
            return RedirectToAction("Index");
        }

        // Nếu model không hợp lệ, hiển thị lại form
        return View(model);
    }

    // Xử lý thêm sản phẩm vào cơ sở dữ liệu
    [HttpPost]
    public IActionResult AddProduct(ProductStoreViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Thêm sản phẩm vào bảng Product
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                imgurl = model.imgurl,
                TheLoaiId = model.TheLoaiId // Liên kết thể loại
            };

            // Lưu sản phẩm vào cơ sở dữ liệu
            _context.Products.Add(product);
            _context.SaveChanges();

            // Thêm mối quan hệ giữa sản phẩm và cửa hàng vào bảng ProductStore
            var productStore = new ProductStore
            {
                ProductId = product.ID, // ID của sản phẩm vừa thêm
                StoreId = model.StoreId, // ID cửa hàng từ model
                Product = product, // Liên kết với sản phẩm
                Store = _context.Stores.Find(model.StoreId) // Tìm cửa hàng bằng ID
            };

            // Lưu mối quan hệ vào cơ sở dữ liệu
            _context.ProductStores.Add(productStore);
            _context.SaveChanges();

            // Chuyển hướng về trang chi tiết cửa hàng
            return RedirectToAction("Details", new { id = model.StoreId });
        }

        // Nếu model không hợp lệ, truyền lại danh sách thể loại và hiển thị lại form
        ViewData["Categories"] = _context.TheLoais.ToList();
        return View(model);
    }
}
