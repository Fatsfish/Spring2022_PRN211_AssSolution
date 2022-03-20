using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly FStoreDBAssignmentContext _context;

        private ProductRepository ProductRepository = new ProductRepository();

        public ProductController(FStoreDBAssignmentContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }
            return View(ProductRepository.GetProducts());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id == null)
            {
                return NotFound();
            }

            var Product = ProductRepository.GetProductByID((int)id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId,ProductName,Weight,UnitPrice,UnitslnStock")] Product Product)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (ModelState.IsValid)
            {
                ProductRepository.InsertProduct(Product);
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id == null)
            {
                return NotFound();
            }

            var Product = ProductRepository.GetProductByID((int)id);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CategoryId,ProductName,Weight,UnitPrice,UnitslnStock")] Product Product)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id != Product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ProductRepository.UpdateProduct(Product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(Product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id == null)
            {
                return NotFound();
            }

            var Product = ProductRepository.GetProductByID((int)id);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            ProductRepository.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
