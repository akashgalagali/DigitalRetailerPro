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
    public class TblUsersController : Controller
    {
        private readonly DigitalRetailersContext _context;

        public TblUsersController(DigitalRetailersContext context)
        {
            _context = context;
        }
        //Get all Customers
        public IActionResult AllCustomers()
        {
            return View( _context.TblUsers.ToList().FindAll(x=>x.Role=="customer"));
        }
        //Get all Sellers
        public IActionResult AllSellers()
        {
            return View(_context.TblUsers.ToList().FindAll(x => x.Role == "seller"));
        }
        // GET: TblUsers
       
        public TblUsers getByMail(string mail)
        {

            return _context.TblUsers.Single(x => x.Email == mail);
        }

        // GET: TblUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TblUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,Role,Location,Mobile")] TblUsers tblUsers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblUsers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(tblUsers);
        }

        // GET: TblUsers/Edit/5
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

            var tblUsers = await _context.TblUsers.FindAsync(id);
            if (tblUsers == null)
            {
                return NotFound();
            }
            return View(tblUsers);
        }

        // POST: TblUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Role,Location,Mobile")] TblUsers tblUsers)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }

            if (id != tblUsers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblUsers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblUsersExists(tblUsers.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (tblUsers.Role == "customer")
                {
                    return RedirectToAction(nameof(GetAvailableLaptop));
                }
                else
                {
                    return RedirectToAction(nameof(GetSoldLaptopOfSeller));
                }
         
               
            }
            return View(tblUsers);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }

            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            string role = _context.TblUsers.Single(x => x.Email == email).Role;

            if (role != "seller")
            {
                return RedirectToAction("login");

            }
            if (id == null)
            {
                return NotFound();
            }

            var tblLaptop = await _context.TblLaptop
                .Include(t => t.Cid)
                .Include(t => t.Sid)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblLaptop == null)
            {
                return NotFound();
            }

            return View(tblLaptop);
        }

        // POST: Laptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }

            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            string role = _context.TblUsers.Single(x => x.Email == email).Role;

            if (role != "seller")
            {
                return RedirectToAction("login");

            }
            var tblLaptop = await _context.TblLaptop.FindAsync(id);
            _context.TblLaptop.Remove(tblLaptop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetSoldLaptopOfSeller));
        }

        private bool TblLaptopExists(int id)
        {
            return _context.TblLaptop.Any(e => e.Id == id);
        }
        //=====================================Extra Actions required for the project====================================


        public bool validateUser(string email, string password,string role)
        {
            List<TblUsers> users = _context.TblUsers.ToList();
            return users.Exists(x => x.Email == email && x.Password == password && x.Role==role);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(TblUsers user)
        {
            if (validateUser(user.Email, user.Password,user.Role))
            {
                HttpContext.Session.SetString("email", user.Email);
                if (user.Role == "seller")
                {
                    return RedirectToAction("GetSoldLaptopOfSeller", "TblUsers", new { uname = user.Name });
                }
                else
                {
                    return RedirectToAction("GetAvailableLaptop", "TblUsers", new { uname = user.Name });
                }

            }
            else
                ViewBag.msg = "Invalid input credentials...";
            return View();
        }

        private bool TblUsersExists(int id)
        {
            return _context.TblUsers.Any(e => e.Id == id);
        }

        //purchase and order

        public async Task<IActionResult> Purchase(int? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            string role = _context.TblUsers.Single(x => x.Email == email).Role;

            if (role!="customer")
            {
                return RedirectToAction("login");

            }
            if (id == null)
            {
                return NotFound();
            }

            var tblLaptop = await _context.TblLaptop.Include(t => t.Cid)
                .Include(t => t.Sid)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblLaptop == null)
            {
                return NotFound();
            }
            
            ViewBag.gst = tblLaptop.Cost * 0.18;
            ViewBag.total=tblLaptop.Cost+ tblLaptop.Cost * 0.18;
            return View(tblLaptop);
        }
        //Order the Laptop
        public async Task<IActionResult> Order(int? id)
        {
            if (HttpContext.Session.GetString("email")==null)
            {
                return RedirectToAction("login");
            }

            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            string role = _context.TblUsers.Single(x => x.Email == email).Role;

            if (role != "customer")
            {
                return RedirectToAction("login");

            }
            if (id == null)
            {
                return NotFound();
            }
            var tblLaptop = await _context.TblLaptop.FindAsync(id);
            if (tblLaptop == null)
            {
                return NotFound();
            }
            ViewData["CidId"] = new SelectList(_context.TblUsers, "Id", "Id", tblLaptop.CidId);
            ViewData["SidId"] = new SelectList(_context.TblUsers, "Id", "Id", tblLaptop.SidId);
            
            return View(tblLaptop);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(int id, [Bind("Id,Brand,Configuration,PaymentMode,Cost,SidId,CidId,Available")] TblLaptop tblLaptop)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            string role = _context.TblUsers.Single(x => x.Email == email).Role;

            if (role != "customer")
            {
                return RedirectToAction("login");

            }
            TblUsers user = getByMail(email);
            if (id != tblLaptop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tblLaptop.Cid = user;
                    tblLaptop.CidId = user.Id;
                    tblLaptop.Available = false;
                  
                    _context.Update(tblLaptop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction(nameof(GetAvailableLaptop));
            }
            ViewBag.Cid = 2;
            ViewData["CidId"] = new SelectList(_context.TblUsers, "Id", "Id", tblLaptop.CidId);
            ViewData["SidId"] = new SelectList(_context.TblUsers, "Id", "Id", tblLaptop.SidId);
            return View(tblLaptop);
        }
        



        //sold laptop of seller
        public IActionResult GetSoldLaptopOfSeller()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            TblUsers user = getByMail(email);
            if (user.Role != "seller")
            {
                return RedirectToAction("login");

            }

            if (user.Id == 0)
            {
                return RedirectToAction("login");
            }

            var digitalRetailersContext = _context.TblLaptop.Include(t => t.Cid).Include(t => t.Sid);
            List<TblLaptop> laptops = digitalRetailersContext.ToList();

            laptops = laptops.FindAll(x => x.Available == false && x.SidId ==user.Id);
            return View(laptops);

        }
     
        //SaleReport
        public async Task<IActionResult> SaleReport(int? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            string role = _context.TblUsers.Single(x => x.Email == email).Role;
            if (role != "seller")
            {
                return RedirectToAction("login");

            }

            if (id == null)
            {
                return NotFound();
            }

            var tblLaptop = await _context.TblLaptop
                .Include(t => t.Cid)
                .Include(t => t.Sid)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblLaptop == null)
            {
                return NotFound();
            }
            return View(tblLaptop);
        }
        //unsold laptop
        public IActionResult UnSoldLaptop()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            TblUsers user = getByMail(email);
            if (user.Role != "seller")
            {
                return RedirectToAction("login");

            }
            if (user.Id == 0)
            {
                return RedirectToAction("login");
            }
            var digitalRetailersContext = _context.TblLaptop.Include(t => t.Cid).Include(t => t.Sid);
            List<TblLaptop> laptops = digitalRetailersContext.ToList();

            laptops = laptops.FindAll(x => x.Available == true && x.Sid.Id == user.Id);
            return View(laptops);

        }
        //Adding new laptop by seller
        public IActionResult AddNewlaptop()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            string role = _context.TblUsers.Single(x => x.Email == email).Role;
            if (role != "seller")
            {
                return RedirectToAction("login");

            }
            return View();
        }

        // POST: Laptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewlaptop([Bind("Id,Brand,Configuration,Cost,SidId,CidId,Available")] TblLaptop tblLaptop)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            TblUsers user = getByMail(email);
            if (user.Role != "seller")
            {
                return RedirectToAction("login");

            }
            if (ModelState.IsValid)
            {
                if (email == null) { }
                else {
                    tblLaptop.Sid = user;
                    tblLaptop.SidId = user.Id;

                _context.Add(tblLaptop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetSoldLaptopOfSeller));
                }
            }
       
            return View(tblLaptop);
        }

     //GET all Available Laptops
        public IActionResult GetAvailableLaptop()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            string role = _context.TblUsers.Single(x => x.Email == email).Role;
            if (role != "customer")
            {
                return RedirectToAction("login");

            }
            var digitalRetailersContext = _context.TblLaptop.Include(t => t.Cid).Include(t => t.Sid);
            List<TblLaptop> laptops = digitalRetailersContext.ToList();
            laptops = laptops.FindAll(x => x.Available == true);
            return View(laptops);
        }


        public IActionResult PurchasedLaptop()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }
            string email = HttpContext.Session.GetString("email");
         
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            string role = _context.TblUsers.Single(x => x.Email == email).Role;

            if (role != "customer")
            {
                return RedirectToAction("login");

            }
            TblUsers user = getByMail(email);
            if (user.Id == 0)
            {
                return RedirectToAction("login");
            }
            var digitalRetailersContext = _context.TblLaptop.Include(t => t.Cid).Include(t => t.Sid);
            List<TblLaptop> laptops = digitalRetailersContext.ToList();

            laptops = laptops.FindAll(x => x.Available == false && x.Cid.Id == user.Id);
            return View(laptops);

        }

        //SaleReport
        public async Task<IActionResult> purchaseReport(int? id)
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("login");
            }

            string email = HttpContext.Session.GetString("email");
            ViewBag.id = _context.TblUsers.Single(x => x.Email == email).Id;
            string role = _context.TblUsers.Single(x => x.Email == email).Role;

            if (role != "customer")
            {
                return RedirectToAction("login");

            }
            if (id == null)
            {
                return NotFound();
            }

            var tblLaptop = await _context.TblLaptop
                .Include(t => t.Cid)
                .Include(t => t.Sid)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblLaptop == null)
            {
                return NotFound();
            }
            return View(tblLaptop);
        }
        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }
    }
}
