using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;
using System.Linq;

namespace Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            // Lấy danh sách các đối tượng targetcustomers
            var customers = _dbContext.targetcustomerss.ToList();

            // Trả về view và truyền danh sách customers
            return View(customers);
        }
    }
}
