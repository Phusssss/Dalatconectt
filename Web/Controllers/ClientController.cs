using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Web.Data;
using Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        // Constructor injection to use ApplicationDbContext
        public ClientController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            // Check if identity is already selected
            var existingIdentity = HttpContext.Session.GetString("UserIdentity");
            if (!string.IsNullOrEmpty(existingIdentity))
            {
                return RedirectToAction("Dashboard"); // Redirect to main dashboard
            }
            return View();
        }

        [HttpPost]
        public IActionResult SaveIdentity(string identity)
        {
            // Validate input
            string[] validIdentities = { "local", "tourist", "student" };
            if (string.IsNullOrEmpty(identity) || !validIdentities.Contains(identity))
            {
                return Json(new { success = false, message = "Invalid identity" });
            }

            // Map identity to database Id
            string customerId = identity switch
            {
                "local" => "1",
                "tourist" => "2",
                "student" => "3",
                _ => throw new InvalidOperationException("Invalid identity")
            };

            // Retrieve the corresponding record
            var targetCustomer = _dbContext.targetcustomerss.FirstOrDefault(x => x.Id == customerId);
            if (targetCustomer != null)
            {
                // Increment SoLuong
                targetCustomer.SoLuong += 1;
                _dbContext.SaveChanges();
            }

            // Save identity to session
            HttpContext.Session.SetString("UserIdentity", identity);

            // Log if needed
            Console.WriteLine($"User identified as: {identity}");

            // Redirect to dashboard
            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Dashboard", "Client")
            });
        }

        public IActionResult Dashboard()
        {
            // Check if identity is selected
            var userIdentity = HttpContext.Session.GetString("UserIdentity");
            if (string.IsNullOrEmpty(userIdentity))
            {
                return RedirectToAction("Index");
            }

            // Pass the identity to the view if needed
            ViewBag.UserIdentity = userIdentity;
            return View();
        }

        // New action for searching products by name
        public IActionResult SearchProduct(string productName, decimal? minPrice = null, decimal? maxPrice = null, string priceSort = "asc")
        {
            if (string.IsNullOrEmpty(productName))
            {
                return View("SearchResult", new ProductSearchViewModel());
            }

            // Lọc sản phẩm theo tên và giá
            var productsQuery = _dbContext.Products
                .Where(p => p.Name.Contains(productName));

            // Áp dụng bộ lọc giá nếu được cung cấp
            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= (double)minPrice.Value); // Ép kiểu minPrice sang double
            }

            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= (double)maxPrice.Value); // Ép kiểu maxPrice sang double
            }

            // Sắp xếp sản phẩm theo giá
            if (priceSort == "desc")
            {
                productsQuery = productsQuery.OrderByDescending(p => p.Price);
            }
            else
            {
                productsQuery = productsQuery.OrderBy(p => p.Price);
            }

            var products = productsQuery.ToList();

            // Lấy các Store liên quan đến sản phẩm này thông qua ProductStore
            var productStores = _dbContext.ProductStores
                .Where(ps => products.Select(p => p.ID).Contains(ps.ProductId))
                .Include(ps => ps.Store)
                .ToList();

            // Chỉ lấy danh sách các Store liên quan
            var stores = productStores.Select(ps => ps.Store).Distinct().ToList();

            var searchResult = new ProductSearchViewModel
            {
                Products = products ?? new List<Product>(),
                Stores = stores ?? new List<Store>(),
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                PriceSort = priceSort  // Thêm PriceSort vào ViewModel để duy trì trạng thái sắp xếp
            };

            return View("SearchResult", searchResult);
        }



    }
}
