using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DreamyDayWeddingPlanningWeb.Controllers
{
    [Authorize(Roles = "Couple")]
    public class GuestsController : Controller
    {
        private readonly IGuestService _guestService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public GuestsController(IGuestService guestService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _guestService = guestService;
            _userManager = userManager;
            _context = context;
        }

        // GET: Guests
        public async Task<IActionResult> Index(string rsvpFilter = "All")
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "User not authenticated.";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var guests = await _guestService.GetGuestsByUserIdAsync(userId);

                // Apply RSVP filter
                if (rsvpFilter == "Yes")
                {
                    guests = guests.Where(g => g.HasRSVPed).ToList();
                }
                else if (rsvpFilter == "No")
                {
                    guests = guests.Where(g => !g.HasRSVPed).ToList();
                }

                // Populate RSVP filter dropdown
                ViewData["RSVPFilter"] = new SelectList(new List<string> { "All", "Yes", "No" }, rsvpFilter);

                return View(guests);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Guests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Guest ID cannot be null.";
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

                var guest = await _guestService.GetGuestByIdAsync(id.Value, userId);
                return View(guest);
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

        // GET: Guests/Create
        public IActionResult Create(int? weddingId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Fetch weddings for the authenticated user
            var weddings = _context.Weddings
                .Where(w => w.UserId == userId && !w.IsDeleted)
                .ToList();

            if (!weddings.Any())
            {
                TempData["ErrorMessage"] = "No weddings found. Please create a wedding first.";
                return RedirectToAction("Dashboard", "Couple");
            }

            // Pre-select the weddingId if provided and valid (i.e., the wedding belongs to the user)
            if (weddingId.HasValue && weddings.Any(w => w.Id == weddingId.Value))
            {
                ViewData["WeddingId"] = new SelectList(weddings, "Id", "Id", weddingId.Value);
            }
            else
            {
                ViewData["WeddingId"] = new SelectList(weddings, "Id", "Id");
            }

            // Populate meal types dropdown
            var mealTypes = new List<string>
            {
                "Vegetarian",
                "Vegan",
                "Non-Vegetarian",
                "Gluten-Free",
                "Dairy-Free",
                "Nut-Free",
                "Halal",
                "Kosher"
            };
            ViewData["MealTypes"] = new SelectList(mealTypes);

            return View();
        }

        // POST: Guests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WeddingId,Name,HasRSVPed,MealPreference,SeatingArrangement")] Guest guest)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.Id == guest.WeddingId && w.UserId == userId);
            if (wedding == null)
            {
                TempData["ErrorMessage"] = "Invalid Wedding ID.";
                ViewData["WeddingId"] = new SelectList(_context.Weddings.Where(w => w.UserId == userId), "Id", "Id", guest.WeddingId);
                var mealTypes = new List<string>
                {
                    "Vegetarian", "Vegan", "Non-Vegetarian", "Gluten-Free", "Dairy-Free", "Nut-Free", "Halal", "Kosher"
                };
                ViewData["MealTypes"] = new SelectList(mealTypes, guest.MealPreference);
                return View(guest);
            }

            // Validate MealPreference and SeatingArrangement
            if (string.IsNullOrWhiteSpace(guest.MealPreference))
            {
                ModelState.AddModelError("MealPreference", "Meal preference is required.");
            }

            if (string.IsNullOrWhiteSpace(guest.SeatingArrangement))
            {
                ModelState.AddModelError("SeatingArrangement", "Seating arrangement is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    guest.WeddingId = wedding.Id;
                    await _guestService.CreateGuestAsync(guest);
                    TempData["SuccessMessage"] = "Guest created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }

            ViewData["WeddingId"] = new SelectList(_context.Weddings.Where(w => w.UserId == userId), "Id", "Id", guest.WeddingId);
            var mealTypesError = new List<string>
            {
                "Vegetarian", "Vegan", "Non-Vegetarian", "Gluten-Free", "Dairy-Free", "Nut-Free", "Halal", "Kosher"
            };
            ViewData["MealTypes"] = new SelectList(mealTypesError, guest.MealPreference);
            return View(guest);
        }

        // GET: Guests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Guest ID cannot be null.";
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

                var guest = await _guestService.GetGuestByIdAsync(id.Value, userId);

                // Materialize the Weddings query before passing to SelectList
                var weddings = await _context.Weddings.Where(w => w.UserId == userId).ToListAsync();
                ViewData["WeddingId"] = new SelectList(weddings, "Id", "Id", guest.WeddingId);

                // Populate meal types dropdown
                var mealTypes = new List<string>
                {
                    "Vegetarian",
                    "Vegan",
                    "Non-Vegetarian",
                    "Gluten-Free",
                    "Dairy-Free",
                    "Nut-Free",
                    "Halal",
                    "Kosher"
                };
                ViewData["MealTypes"] = new SelectList(mealTypes, guest.MealPreference);

                return View(guest);
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

        // POST: Guests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WeddingId,Name,HasRSVPed,MealPreference,SeatingArrangement")] Guest guest)
        {
            if (id != guest.Id)
            {
                TempData["ErrorMessage"] = "Guest ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.Id == guest.WeddingId && w.UserId == userId);
            if (wedding == null)
            {
                TempData["ErrorMessage"] = "Invalid Wedding ID.";
                ViewData["WeddingId"] = new SelectList(_context.Weddings.Where(w => w.UserId == userId), "Id", "Id", guest.WeddingId);
                var mealTypes = new List<string>
                {
                    "Vegetarian", "Vegan", "Non-Vegetarian", "Gluten-Free", "Dairy-Free", "Nut-Free", "Halal", "Kosher"
                };
                ViewData["MealTypes"] = new SelectList(mealTypes, guest.MealPreference);
                return View(guest);
            }

            // Validate MealPreference and SeatingArrangement
            if (string.IsNullOrWhiteSpace(guest.MealPreference))
            {
                ModelState.AddModelError("MealPreference", "Meal preference is required.");
            }

            if (string.IsNullOrWhiteSpace(guest.SeatingArrangement))
            {
                ModelState.AddModelError("SeatingArrangement", "Seating arrangement is required.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    guest.WeddingId = wedding.Id;
                    await _guestService.UpdateGuestAsync(guest);
                    TempData["SuccessMessage"] = "Guest updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    // Re-populate dropdowns and return the view with the current guest data
                    ViewData["WeddingId"] = new SelectList(_context.Weddings.Where(w => w.UserId == userId), "Id", "Id", guest.WeddingId);
                    var mealTypes = new List<string>
                    {
                        "Vegetarian", "Vegan", "Non-Vegetarian", "Gluten-Free", "Dairy-Free", "Nut-Free", "Halal", "Kosher"
                    };
                    ViewData["MealTypes"] = new SelectList(mealTypes, guest.MealPreference);
                    return View(guest);
                }
            }

            // If ModelState is invalid, re-populate dropdowns and return the view
            ViewData["WeddingId"] = new SelectList(_context.Weddings.Where(w => w.UserId == userId), "Id", "Id", guest.WeddingId);
            var mealTypesError = new List<string>
            {
                "Vegetarian", "Vegan", "Non-Vegetarian", "Gluten-Free", "Dairy-Free", "Nut-Free", "Halal", "Kosher"
            };
            ViewData["MealTypes"] = new SelectList(mealTypesError, guest.MealPreference);
            return View(guest);
        }

        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Guest ID cannot be null.";
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

                var guest = await _guestService.GetGuestByIdAsync(id.Value, userId);
                return View(guest);
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

        // POST: Guests/Delete/5
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

                await _guestService.DeleteGuestAsync(id, userId);
                TempData["SuccessMessage"] = "Guest deleted successfully.";
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