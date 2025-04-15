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
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not authenticated.";
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }

                var wedding = await _context.Weddings
                    .Where(w => w.UserId == user.Id && !w.IsDeleted)
                    .FirstOrDefaultAsync();

                if (wedding == null)
                {
                    TempData["ErrorMessage"] = "No wedding found for this user. Please create a wedding first.";
                    return RedirectToAction("Dashboard", "Couple");
                }

                var guests = await _guestService.GetGuestsByUserIdAsync(user.Id);

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
                ViewBag.WeddingId = wedding.Id; // Pass WeddingId for Create/Edit links

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
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Fetch the wedding for the authenticated user
            var wedding = await _context.Weddings
                .Where(w => w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "No wedding found for this user. Please create a wedding first.";
                return RedirectToAction("Dashboard", "Couple");
            }

            // Create a new Guest model with the WeddingId pre-filled
            var guest = new Guest
            {
                WeddingId = wedding.Id
            };

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

            return View(guest);
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
                    var mealTypes = new List<string>
                    {
                        "Vegetarian", "Vegan", "Non-Vegetarian", "Gluten-Free", "Dairy-Free", "Nut-Free", "Halal", "Kosher"
                    };
                    ViewData["MealTypes"] = new SelectList(mealTypes, guest.MealPreference);
                    return View(guest);
                }
            }

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