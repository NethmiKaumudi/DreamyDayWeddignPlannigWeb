using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.EntityFrameworkCore;
using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using System.Linq;
using DreamyDayWeddingPlanningWeb.Data;

namespace DreamyDayWeddingPlanningWeb.Controllers
{
    [Authorize(Roles = "Couple")]
    public class BudgetsController : Controller
    {
        private readonly IBudgetService _budgetService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public BudgetsController(
            IBudgetService budgetService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _budgetService = budgetService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "User not authenticated.";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var budgets = await _budgetService.GetBudgetsByUserIdAsync(userId);
                var wedding = await _context.Weddings
                    .Where(w => w.UserId == userId && !w.IsDeleted)
                    .FirstOrDefaultAsync();

                if (wedding == null)
                {
                    TempData["ErrorMessage"] = "No wedding found. Please create a wedding first.";
                    return RedirectToAction("Dashboard", "Couple");
                }

                ViewBag.WeddingId = wedding.Id;
                ViewBag.TotalBudget = wedding.TotalBudget; // Pass the wedding's total budget
                return View(budgets);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Budget ID cannot be null.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "User not authenticated.";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var budget = await _budgetService.GetBudgetByIdAsync(id.Value, userId);
                if (budget == null)
                {
                    TempData["ErrorMessage"] = "Budget item not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(budget);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create(int weddingId)
        {
            var budget = new Budget { WeddingId = weddingId };
            ViewBag.Categories = new List<string>
            {
                "Venue", "Catering", "Photography", "Decorations", "Entertainment", "Attire", "Invitations", "Other"
            };
            return View(budget);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WeddingId,Category,AllocatedAmount,SpentAmount")] Budget budget)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == budget.WeddingId && w.UserId == userId && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "Invalid Wedding ID.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _budgetService.CreateBudgetAsync(budget);
                    TempData["SuccessMessage"] = "Budget item created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }

            ViewBag.Categories = new List<string>
            {
                "Venue", "Catering", "Photography", "Decorations", "Entertainment", "Attire", "Invitations", "Other"
            };
            return View(budget);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Budget ID cannot be null.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "User not authenticated.";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var budget = await _budgetService.GetBudgetByIdAsync(id.Value, userId);
                ViewBag.Categories = new List<string>
                {
                    "Venue", "Catering", "Photography", "Decorations", "Entertainment", "Attire", "Invitations", "Other"
                };
                return View(budget);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WeddingId,Category,AllocatedAmount,SpentAmount")] Budget budget)
        {
            if (id != budget.Id)
            {
                TempData["ErrorMessage"] = "Budget ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _budgetService.UpdateBudgetAsync(budget);
                    TempData["SuccessMessage"] = "Budget item updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }

            ViewBag.Categories = new List<string>
            {
                "Venue", "Catering", "Photography", "Decorations", "Entertainment", "Attire", "Invitations", "Other"
            };
            return View(budget);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Budget ID cannot be null.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "User not authenticated.";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var budget = await _budgetService.GetBudgetByIdAsync(id.Value, userId);
                return View(budget);
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "User not authenticated.";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                await _budgetService.DeleteBudgetAsync(id, userId);
                TempData["SuccessMessage"] = "Budget item deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }
    }
}