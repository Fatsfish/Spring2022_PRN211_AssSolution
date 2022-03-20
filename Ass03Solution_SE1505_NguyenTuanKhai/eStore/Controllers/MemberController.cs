using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eStore.Controllers
{
    public class MemberController : Controller
    {
        private readonly FStoreDBAssignmentContext _context;

        private MemberRepository memberRepository = new MemberRepository();

        public MemberController(FStoreDBAssignmentContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }
            return View(memberRepository.GetMembers().ToList());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id == null)
            {
                return NotFound();
            }

            var Member =  memberRepository.GetMemberByID((int)id);
            if (Member == null)
            {
                return NotFound();
            }

            return View(Member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,CompanyName,City,Country,Password,Email")] Member Member)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (ModelState.IsValid)
            {
                memberRepository.InsertMember(Member);
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id == null)
            {
                return NotFound();
            }

            var Member = memberRepository.GetMemberByID((int)id);
            if (Member == null)
            {
                return NotFound();
            }
            return View(Member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,CompanyName,City,Country,Password,Email")] Member Member)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id != Member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    memberRepository.UpdateMember(Member);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(Member.MemberId))
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
            return View(Member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            if (id == null)
            {
                return NotFound();
            }

            var Member = memberRepository.GetMemberByID((int)id);

            if (Member == null)
            {
                return NotFound();
            }

            return View(Member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null) { HttpContext.Session.SetString("error", "Please login first to access!"); return Redirect("/"); }

            memberRepository.DeleteMember(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }
    }
}
