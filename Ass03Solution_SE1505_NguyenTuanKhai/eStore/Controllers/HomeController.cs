using BusinessObject;
using eStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly FStoreDBAssignmentContext _context;

        public HomeController(ILogger<HomeController> logger, FStoreDBAssignmentContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,CompanyName,Country,Address,Password,Email")] Member Member)
        {
            if (ModelState.IsValid)
            {
                Member.MemberId = _context.Members.Count() + 100;
                _context.Add(Member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Password,Email")] Member Member)
        {
            if (ModelState.IsValid)
            {
                string json = string.Empty;

                // read json string from file
                using (StreamReader reader = new StreamReader("appsettings.json"))
                {
                    json = reader.ReadToEnd();
                }

                JavaScriptSerializer jss = new JavaScriptSerializer();

                // convert json string to dynamic type
                var obj = jss.Deserialize<dynamic>(json);

                // get contents
                string Email = obj["DefaultAccount"]["Email"];
                string Password = obj["DefaultAccount"]["password"];

                if (Email.ToLower().Equals(Member.Email.ToLower()) && Password.ToLower().Equals(Member.Password))
                {
                    HttpContext.Session.SetString("name", Email);
                    HttpContext.Session.SetInt32("id", -68);
                    return RedirectToAction("Index");
                }

                var check = _context.Members.FirstOrDefault(o => o.Email.ToLower().Equals(Member.Email.ToLower()) && o.Password.Equals(Member.Password));
                if (check != null)
                {
                    HttpContext.Session.SetString("name", check.Email);
                    HttpContext.Session.SetInt32("id", check.MemberId);
                    return RedirectToAction("Index");
                }
            }
            return View(Member);
        }
    }
}
