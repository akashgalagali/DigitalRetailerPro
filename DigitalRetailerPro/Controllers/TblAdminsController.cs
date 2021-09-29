using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalRetailerPro.Models;
using Microsoft.AspNetCore.Http;

namespace DigitalRetailerPro.Controllers
{
    public class TblAdminsController : Controller
    {
        private readonly DigitalRetailersContext _context;

        public TblAdminsController(DigitalRetailersContext context)
        {
            _context = context;
        }

        // GET: TblAdmins
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblAdmin.Single(x => x.Email == email).Id;
            return View(await _context.TblAdmin.ToListAsync());
        }

        // GET: TblAdmins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var tblAdmin = await _context.TblAdmin
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblAdmin == null)
            {
                return NotFound();
            }

            return View(tblAdmin);
        }

        // GET: TblAdmins/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            return View();
        }

        // POST: TblAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password")] TblAdmin tblAdmin)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            if (ModelState.IsValid)
            {
                _context.Add(tblAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblAdmin);
        }

        // GET: TblAdmins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var tblAdmin = await _context.TblAdmin.FindAsync(id);
            if (tblAdmin == null)
            {
                return NotFound();
            }
            return View(tblAdmin);
        }

        // POST: TblAdmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password")] TblAdmin tblAdmin)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            if (id != tblAdmin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblAdminExists(tblAdmin.Id))
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
            return View(tblAdmin);
        }

        // GET: TblAdmins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var tblAdmin = await _context.TblAdmin
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblAdmin == null)
            {
                return NotFound();
            }

            return View(tblAdmin);
        }

        // POST: TblAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            var tblAdmin = await _context.TblAdmin.FindAsync(id);
            _context.TblAdmin.Remove(tblAdmin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
 //================================= Extra Actions required for project====================================
        private bool TblAdminExists(int id)
        {
            return _context.TblAdmin.Any(e => e.Id == id);
        }
        public bool validateUser(string email, string password)
        {
            List<TblAdmin> users = _context.TblAdmin.ToList();
            return users.Exists(x => x.Email == email && x.Password == password);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(TblAdmin admin)
        {
            if (validateUser(admin.Email, admin.Password))
            {

                HttpContext.Session.SetString("email", admin.Email);
                return RedirectToAction("Index", "TblAdmins");
            }
            else
                ViewBag.msg = "Invalid input credentials...";
            return View();
        }
        //GET all Available Laptops
        public IActionResult GetAvailableLaptop()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblAdmin.Single(x => x.Email == email).Id;
            var digitalRetailersContext = _context.TblLaptop.Include(t => t.Cid).Include(t => t.Sid);
            List<TblLaptop> laptops = digitalRetailersContext.ToList();
            laptops = laptops.FindAll(x => x.Available == true);
            return View(laptops);
        }
        //GET all Sold Laptops
        public IActionResult GetSoldLaptop()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblAdmin.Single(x => x.Email == email).Id;
            var digitalRetailersContext = _context.TblLaptop.Include(t => t.Cid).Include(t => t.Sid);
            List<TblLaptop> laptops = digitalRetailersContext.ToList();
            laptops = laptops.FindAll(x => x.Available == false);
            return View(laptops);
        }
        //Get all Customers
        public IActionResult AllCustomers()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblAdmin.Single(x => x.Email == email).Id;
            return View(_context.TblUsers.ToList().FindAll(x => x.Role == "customer"));
        }
        //Get all Sellers
        public IActionResult AllSellers()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblAdmin.Single(x => x.Email == email).Id;
            return View(_context.TblUsers.ToList().FindAll(x => x.Role == "seller"));
        }
        //=====================================
        public async Task<IActionResult> AllUsers()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblAdmin.Single(x => x.Email == email).Id;

            return View(await _context.TblUsers.ToListAsync());
        }

        // GET: TblUsers/Details/5
        public async Task<IActionResult> UserDetails(int? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblAdmin.Single(x => x.Email == email).Id;

            if (id == null)
            {
                return NotFound();
            }

            var tblUsers = await _context.TblUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUsers == null)
            {
                return NotFound();
            }

            return View(tblUsers);
        }

        

        //=====================================================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }
    }
}
