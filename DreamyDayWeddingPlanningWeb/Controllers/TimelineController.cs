using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.EntityFrameworkCore;
using DreamyDayWeddingPlanningWeb.Data;
using System.Linq;
using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using DreamyDayWeddingPlanningWeb.Business.Services;

namespace DreamyDayWeddingPlanningWeb.Controllers
{
    [Authorize(Roles = "Couple")]
    public class TimelineController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWeddingTimeLineService _timelineService;

        public TimelineController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IWeddingTimeLineService timelineService)
        {
            _userManager = userManager;
            _context = context;
            _timelineService = timelineService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wedding = await _context.Weddings
                .Where(w => w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "No wedding found for this user. Please create a wedding first.";
                return RedirectToAction("Dashboard", "Couple");
            }

            var timelineEvents = await _timelineService.GetTimelineEventsByWeddingIdAsync(wedding.Id);
            ViewBag.WeddingId = wedding.Id; // Pass weddingId for Create/Edit links
            ViewBag.WeddingDate = wedding.WeddingDate; // Pass wedding date for display
            return View(timelineEvents);
        }

        public async Task<IActionResult> Create(int weddingId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Create", "Timeline", new { weddingId }) });
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == weddingId && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "The specified wedding was not found or you do not have access to it.";
                return RedirectToAction("Dashboard", "Couple");
            }

            ViewBag.WeddingId = weddingId; // Set WeddingId for the view
            ViewBag.WeddingDate = wedding.WeddingDate;
            var timelineEvent = new TimelineEvent { WeddingId = weddingId };
            return View(timelineEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimelineEvent timelineEvent)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == timelineEvent.WeddingId && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "The specified wedding was not found or you do not have access to it.";
                return RedirectToAction("Dashboard", "Couple");
            }

            if (ModelState.IsValid)
            {
                await _timelineService.CreateTimelineEventAsync(timelineEvent);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.WeddingId = timelineEvent.WeddingId; // Ensure WeddingId is set if validation fails
            ViewBag.WeddingDate = wedding.WeddingDate;
            return View(timelineEvent);
        }

        public async Task<IActionResult> Edit(int id, int weddingId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Edit", "Timeline", new { id, weddingId }) });
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == weddingId && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "The specified wedding was not found or you do not have access to it.";
                return RedirectToAction("Dashboard", "Couple");
            }

            var timelineEvent = await _timelineService.GetTimelineEventByIdAsync(id, wedding.Id);
            if (timelineEvent == null)
            {
                return NotFound();
            }

            ViewBag.WeddingId = weddingId; // Set WeddingId for the view
            ViewBag.WeddingDate = wedding.WeddingDate;
            return View(timelineEvent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int weddingId, TimelineEvent timelineEvent)
        {
            if (id != timelineEvent.Id)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == weddingId && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "The specified wedding was not found or you do not have access to it.";
                return RedirectToAction("Dashboard", "Couple");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEvent = await _timelineService.GetTimelineEventByIdAsync(id, wedding.Id);
                    if (existingEvent == null)
                    {
                        return NotFound();
                    }

                    existingEvent.EventName = timelineEvent.EventName;
                    existingEvent.StartTime = timelineEvent.StartTime;
                    existingEvent.EndTime = timelineEvent.EndTime;
                    existingEvent.Description = timelineEvent.Description;
                    existingEvent.IsDeleted = timelineEvent.IsDeleted;

                    await _timelineService.UpdateTimelineEventAsync(existingEvent);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingEvent = await _timelineService.GetTimelineEventByIdAsync(id, wedding.Id);
                    if (existingEvent == null)
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

            ViewBag.WeddingId = weddingId; // Ensure WeddingId is set if validation fails
            ViewBag.WeddingDate = wedding.WeddingDate;
            return View(timelineEvent);
        }

        public async Task<IActionResult> Details(int id, int weddingId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Details", "Timeline", new { id, weddingId }) });
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == weddingId && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "The specified wedding was not found or you do not have access to it.";
                return RedirectToAction("Dashboard", "Couple");
            }

            var timelineEvent = await _timelineService.GetTimelineEventByIdAsync(id, wedding.Id);
            if (timelineEvent == null)
            {
                return NotFound();
            }

            ViewBag.WeddingId = weddingId; // Set WeddingId for the view
            return View(timelineEvent);
        }

        public async Task<IActionResult> Delete(int id, int weddingId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Delete", "Timeline", new { id, weddingId }) });
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == weddingId && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "The specified wedding was not found or you do not have access to it.";
                return RedirectToAction("Dashboard", "Couple");
            }

            var timelineEvent = await _timelineService.GetTimelineEventByIdAsync(id, wedding.Id);
            if (timelineEvent == null)
            {
                return NotFound();
            }

            ViewBag.WeddingId = weddingId; // Set WeddingId for the view
            return View(timelineEvent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int weddingId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var wedding = await _context.Weddings
                .Where(w => w.Id == weddingId && w.UserId == user.Id && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "The specified wedding was not found or you do not have access to it.";
                return RedirectToAction("Dashboard", "Couple");
            }

            var timelineEvent = await _timelineService.GetTimelineEventByIdAsync(id, wedding.Id);
            if (timelineEvent == null)
            {
                return NotFound();
            }

            await _timelineService.DeleteTimelineEventAsync(id, wedding.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}