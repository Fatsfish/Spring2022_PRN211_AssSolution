using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace eStore.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly FStoreDBAssignmentContext _context;

        private OrderDetailRepository OrderDetailRepository = new OrderDetailRepository();

        public OrderDetailController(FStoreDBAssignmentContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }
            return View(OrderDetailRepository.GetOrderDetails());
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id, int? id1)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id == null)
            {
                return NotFound();
            }

            var OrderDetail = OrderDetailRepository.GetOrderDetailByID((int)id, (int)id1);
            if (OrderDetail == null)
            {
                return NotFound();
            }

            return View(OrderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ProductId,UnitPrice,Quantity,Discount")] OrderDetail OrderDetail)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (ModelState.IsValid)
            {
                OrderDetailRepository.InsertOrderDetail(OrderDetail);
                return RedirectToAction(nameof(Index));
            }
            return View(OrderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id, int? id1)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id == null)
            {
                return NotFound();
            }

            var OrderDetail = OrderDetailRepository.GetOrderDetailByID((int)id, (int)id1);
            if (OrderDetail == null)
            {
                return NotFound();
            }
            return View(OrderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int id1, [Bind("OrderId,ProductId,UnitPrice,Quantity,Discount")] OrderDetail OrderDetail)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id != OrderDetail.OrderId || id1 != OrderDetail.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    OrderDetailRepository.UpdateOrderDetail(OrderDetail);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(OrderDetail.OrderId,OrderDetail.ProductId))
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
            return View(OrderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id, int? id1)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id == null)
            {
                return NotFound();
            }

            var OrderDetail = OrderDetailRepository.GetOrderDetailByID((int)id, (int) id1);

            if (OrderDetail == null)
            {
                return NotFound();
            }

            return View(OrderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int id1)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            OrderDetailRepository.DeleteOrderDetail(id,id1);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id, int id1)
        {
            return _context.OrderDetails.Any(e => e.OrderId == id && e.ProductId==id1);
        }
    }
}
