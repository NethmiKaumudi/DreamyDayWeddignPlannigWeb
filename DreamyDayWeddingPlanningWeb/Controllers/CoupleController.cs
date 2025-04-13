using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.EntityFrameworkCore;
using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using DreamyDayWeddingPlanningWeb.Data;
using System.Linq;

namespace DreamyDayWeddingPlanningWeb.Controllers
{
    [Authorize(Roles = "Couple")]
    public class CoupleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWeddingService _weddingService;
        private readonly IGuestService _guestService;
        private readonly IBudgetService _budgetService;
        private readonly ApplicationDbContext _context;

        public CoupleController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWeddingService weddingService,
            IGuestService guestService,
            IBudgetService budgetService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _weddingService = weddingService;
            _guestService = guestService;
            _budgetService = budgetService;
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var weddings = await _context.Weddings
                .Where(w => w.UserId == user.Id && !w.IsDeleted)
                .Include(w => w.Planner)
                .ToListAsync();

            var tasks = await _context.WeddingTasks
                .Where(t => t.UserId == user.Id && !t.IsDeleted && !t.IsCompleted)
                .OrderBy(t => t.Deadline)
                .ToListAsync();

            var guests = await _guestService.GetGuestsByUserIdAsync(user.Id);
            var guestCount = guests.Count;

            if (weddings == null || weddings.Count == 0)
            {
                ViewBag.HasWedding = false;

                // Modified: Fetch planners directly from aspnetusers based on the 'Role' column
                var planners = await _context.Users
                    .Where(u => u.Role == "Planner")
                    .Select(u => new { u.Id, u.UserName, u.ContactNumber })
                    .ToListAsync();

                ViewBag.Planners = planners;
                return View("Dashboard", null);
            }

            var firstWedding = weddings.First();
            bool budgetExceeded = await _budgetService.CheckBudgetExceededAsync(firstWedding.Id);
            if (budgetExceeded)
            {
                TempData["BudgetExceededMessage"] = "Warning: You have exceeded your wedding budget!";
            }

            ViewBag.HasWedding = true;
            ViewBag.Weddings = weddings;
            ViewBag.Tasks = tasks;
            ViewBag.GuestCount = guestCount;
            return View("Dashboard", firstWedding);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWedding(string weddingTitle, DateTime weddingDate, decimal totalBudget, string plannerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (string.IsNullOrEmpty(plannerId))
            {
                ModelState.AddModelError("plannerId", "Please select a planner.");
                // Modified: Fetch planners directly from aspnetusers based on the 'Role' column
                var planners = await _context.Users
                    .Where(u => u.Role == "Planner")
                    .Select(u => new { u.Id, u.UserName, u.ContactNumber })
                    .ToListAsync();
                ViewBag.Planners = planners;
                ViewBag.HasWedding = false;

                return View("Dashboard", null);
            }

            var wedding = new Wedding
            {
                UserId = user.Id,
                WeddingDate = weddingDate,
                TotalBudget = totalBudget,
                SpentBudget = 0,
                PlannerId = plannerId,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                Progress = 0
            };

            await _weddingService.CreateWeddingAsync(wedding);
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> EditWedding(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == id && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                return RedirectToAction("Dashboard");
            }

            // Modified: Fetch planners directly from aspnetusers based on the 'Role' column
            var planners = await _context.Users
                .Where(u => u.Role == "Planner")
                .Select(u => new { u.Id, u.UserName, u.ContactNumber })
                .ToListAsync();
            ViewBag.Planners = planners;
            return View(wedding);
        }

        [HttpPost]
        public async Task<IActionResult> EditWedding(Wedding wedding)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingWedding = await _context.Weddings
                .Where(w => w.Id == wedding.Id && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (existingWedding == null)
            {
                return RedirectToAction("Dashboard");
            }

            existingWedding.WeddingDate = wedding.WeddingDate;
            existingWedding.PlannerId = wedding.PlannerId;
            await _weddingService.UpdateWeddingAsync(existingWedding);
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> DeleteWedding(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == id && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                return RedirectToAction("Dashboard");
            }

            await _weddingService.DeleteWeddingAsync(wedding.Id);
            return RedirectToAction("Dashboard");
        }

        public IActionResult GuestList()
        {
            return RedirectToAction("Index", "Guests");
        }

        public IActionResult Checklist()
        {
            return RedirectToAction("Index", "WeddingTasks");
        }

        public IActionResult Budget()
        {
            return RedirectToAction("Index", "Budgets");
        }

        public IActionResult Timeline()
        {
            return View();
        }

        public IActionResult VendorCatalog()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}