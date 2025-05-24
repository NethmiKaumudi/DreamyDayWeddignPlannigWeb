using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using DinkToPdf.Contracts;
using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Models.Planner;
using DreamyDayWeddingPlanningWeb.Models;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using DreamyDayWeddingPlanningWeb.Services;

namespace DreamyDayWeddingPlanningWeb.Controllers
{
    [Authorize(Roles = "Planner")]
    public class PlannerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConverter _pdfConverter;
        private readonly IViewRenderService _viewRenderService;

        public PlannerController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IConverter pdfConverter,
            IViewRenderService viewRenderService)
        {
            _context = context;
            _userManager = userManager;
            _pdfConverter = pdfConverter;
            _viewRenderService = viewRenderService;
        }


        [HttpGet]
        public async Task<IActionResult> Reports()
        {
            var weddings = await _context.Weddings.Where(w => !w.IsDeleted).ToListAsync();
            ViewBag.Weddings = weddings;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(int weddingId, string reportType)
        {
            var wedding = await _context.Weddings.FirstOrDefaultAsync(w => w.Id == weddingId);
            if (wedding == null) return NotFound();

            var vendors = await _context.WeddingVendors
                .Include(wv => wv.Vendor)
                .Where(wv => wv.WeddingId == weddingId)
                .Select(wv => new WeddingVendorReportItem
                {
                    Name = wv.Vendor.Name,
                    Category = wv.Vendor.Category,
                    Price = wv.Vendor.Price,
                    Notes = wv.Notes ?? "",
                    Priority = wv.Priority
                }).ToListAsync();

            var tasks = await _context.WeddingTasks
                .Include(t => t.AssignedToUser)
                .Where(t => t.WeddingId == weddingId)
                .Select(t => new WeddingTaskReportItem
                {
                    TaskName = t.TaskName,
                    Deadline = t.Deadline,
                    IsCompleted = t.IsCompleted,
                    AssignedUserName = t.AssignedToUser != null ? t.AssignedToUser.UserName : "Unassigned"
                }).ToListAsync();

            var model = new WeddingReportViewModel
            {
                Wedding = wedding,
                Vendors = vendors,
                Tasks = tasks
            };

            // Update the method call to explicitly cast the model to resolve ambiguity
            var html = await _viewRenderService.RenderToStringAsync("Planner/WeddingReportTemplate", (object)model);

            if (reportType == "view")
                return Content(html, "text/html");

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = { new ObjectSettings { HtmlContent = html } }
            };

            var pdfBytes = _pdfConverter.Convert(pdf);
            return File(pdfBytes, "application/pdf", "WeddingReport.pdf");
        }

        // ✅ DASHBOARD
        public async Task<IActionResult> Dashboard()
        {
            var planner = await _userManager.GetUserAsync(User);
            if (planner == null) return Unauthorized();

            var weddings = await _context.Weddings
                .Where(w => w.PlannerId == planner.Id && !w.IsDeleted)
                .ToListAsync();

            var userMap = await _context.Users.ToDictionaryAsync(u => u.Id, u => u.UserName);

            // Retrieve vendor assignments for budgeting
            var vendorAssignments = await _context.WeddingVendors.ToListAsync();
            var allVendors = await _context.Vendors.ToListAsync();

            var allWeddings = weddings
                .OrderBy(w => w.WeddingDate)
                .Select(w =>
                {
                    var coupleName = userMap.ContainsKey(w.UserId) ? userMap[w.UserId] : "Unknown";
                    var vendorIds = vendorAssignments
                        .Where(v => v.WeddingId == w.Id)
                        .Select(v => v.VendorId)
                        .ToList();

                    var budget = allVendors
                        .Where(v => vendorIds.Contains(v.Id))
                        .Sum(v => v.Price);

                    return new WeddingSummaryViewModel
                    {
                        WeddingId = w.Id,
                        CoupleName = coupleName,
                        WeddingDate = w.WeddingDate,
                        IsCompleted = w.IsCompleted,
                        Budget = budget
                    };
                }).ToList();

            var tasks = await _context.WeddingTasks
                .Where(t => t.UserId == planner.Id && !t.IsDeleted)
                .ToListAsync();

            var upcomingTasks = tasks
                .Where(t => !t.IsCompleted && t.Deadline > DateTime.Now)
                .OrderBy(t => t.Deadline)
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    TaskName = t.TaskName,
                    Deadline = t.Deadline,
                    IsCompleted = t.IsCompleted
                }).Take(3).ToList();

            var viewModel = new PlannerDashboardViewModel
            {
                TotalWeddings = weddings.Count,
                TasksCompleted = tasks.Count(t => t.IsCompleted),
                TasksTotal = tasks.Count,
                UpcomingTasks = upcomingTasks,
                AllWeddings = allWeddings,
                UpcomingWeddings = allWeddings.Where(w => w.WeddingDate > DateTime.Now).Take(3).ToList(),
                Notifications = new List<NotificationViewModel>
        {
            new NotificationViewModel { Message = "New vendor added", Date = DateTime.Now.AddHours(-1) },
            new NotificationViewModel { Message = "2 tasks pending", Date = DateTime.Now.AddDays(-1) },
        }
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ToggleWeddingStatus(int id)
        {
            var wedding = await _context.Weddings.FindAsync(id);
            if (wedding == null) return NotFound();

            wedding.IsCompleted = !wedding.IsCompleted;
            _context.Weddings.Update(wedding);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        // ✅ TASKS
        [HttpGet]
        public async Task<IActionResult> ManageTasks()
        {
            var planner = await _userManager.GetUserAsync(User);
            if (planner == null) return Unauthorized();

            var tasks = await _context.WeddingTasks
                .Include(t => t.Wedding)
                .Include(t => t.AssignedToUser)
                .Where(t => t.UserId == planner.Id && !t.IsDeleted)
                .OrderBy(t => t.Deadline)
                .Select(t => new TaskViewModel
                {
                    Id = t.Id,
                    TaskName = t.TaskName,
                    Deadline = t.Deadline,
                    IsCompleted = t.IsCompleted,
                    WeddingName = t.Wedding != null ? t.Wedding.WeddingTitle : "N/A",
                    AssignedUserName = t.AssignedToUser != null ? t.AssignedToUser.UserName : "N/A"
                }).ToListAsync();

            ViewBag.Weddings = await _context.Weddings
                .Where(w => w.PlannerId == planner.Id && !w.IsDeleted)
                .ToListAsync();

            ViewBag.Users = await _userManager.Users.ToListAsync();

            return View(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(string TaskName, DateTime Deadline, int WeddingId, string AssignedToUserId)
        {
            var planner = await _userManager.GetUserAsync(User);
            if (planner == null) return Unauthorized();

            var task = new WeddingTask
            {
                TaskName = TaskName,
                Deadline = Deadline,
                IsCompleted = false,
                UserId = planner.Id,
                WeddingId = WeddingId,
                AssignedToUserId = AssignedToUserId,
                IsDeleted = false
            };

            _context.WeddingTasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleTaskStatus(int id)
        {
            var task = await _context.WeddingTasks.FindAsync(id);
            if (task == null) return NotFound();

            task.IsCompleted = !task.IsCompleted;
            _context.WeddingTasks.Update(task);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // ✅ VENDORS
        public async Task<IActionResult> ManageVendors(int? weddingId)
        {
            var planner = await _userManager.GetUserAsync(User);
            if (planner == null) return Unauthorized();

            var vendors = await _context.Vendors.Where(v => !v.IsDeleted).ToListAsync();
            var weddingVendors = await _context.WeddingVendors.Include(wv => wv.Wedding).ToListAsync();
            var weddings = await _context.Weddings.Where(w => w.PlannerId == planner.Id && !w.IsDeleted).ToListAsync();

            var viewModel = vendors.Select(v =>
            {
                var assignment = weddingVendors.FirstOrDefault(wv => wv.VendorId == v.Id && (!weddingId.HasValue || wv.WeddingId == weddingId));
                return new VendorViewModel
                {
                    Id = v.Id,
                    Name = v.Name,
                    Category = v.Category,
                    Description = v.Description,
                    Price = Math.Round(v.Price, 2),
                    IsApproved = v.IsApproved,
                    Reviews = v.Reviews,
                    IsAssigned = assignment != null,
                    Notes = assignment?.Notes,
                    Priority = assignment?.Priority,
                    WeddingId = assignment?.WeddingId,
                    WeddingTitle = assignment?.Wedding?.WeddingTitle
                };
            }).ToList();

            ViewBag.Weddings = weddings;
            ViewBag.SelectedWeddingId = weddingId;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BookVendor(int vendorId, int weddingId, string? notes, int? priority)
        {
            var exists = await _context.WeddingVendors.AnyAsync(wv => wv.VendorId == vendorId && wv.WeddingId == weddingId);
            if (!exists)
            {
                _context.WeddingVendors.Add(new WeddingVendor
                {
                    VendorId = vendorId,
                    WeddingId = weddingId,
                    Notes = notes,
                    Priority = priority ?? 1
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageVendors");
        }

        [HttpPost]
        public async Task<IActionResult> AddVendor(VendorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vendor = new Vendor
                {
                    Name = model.Name,
                    Category = model.Category,
                    Description = model.Description,
                    Price = model.Price,
                    IsApproved = true,
                    IsDeleted = false
                };

                _context.Vendors.Add(vendor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageVendors");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor != null)
            {
                vendor.IsDeleted = true;
                _context.Vendors.Update(vendor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageVendors");
        }

        [HttpGet]
        public async Task<IActionResult> FilterVendors(int? weddingId)
        {
            return await ManageVendors(weddingId); // ✅ Reuse existing view
        }



        // ✅ TIMELINE
        [HttpGet]
        public async Task<IActionResult> Timeline(int? weddingId)
        {
            var planner = await _userManager.GetUserAsync(User);
            if (planner == null) return Unauthorized();

            var weddings = await _context.Weddings.Where(w => w.PlannerId == planner.Id && !w.IsDeleted).ToListAsync();
            ViewBag.Weddings = weddings;
            ViewBag.SelectedWeddingId = weddingId;

            if (!weddingId.HasValue) return View(new List<TimelineEventViewModel>());

            var events = await _context.TimelineEvents
                .Where(e => e.WeddingId == weddingId && !e.IsDeleted)
                .OrderBy(e => e.StartTime)
                .Select(e => new TimelineEventViewModel
                {
                    Id = e.Id,
                    EventName = e.EventName,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Description = e.Description
                }).ToListAsync();

            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> AddTimelineEvent(TimelineEventViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Timeline", new { weddingId = model.WeddingId });

            _context.TimelineEvents.Add(new TimelineEvent
            {
                WeddingId = model.WeddingId,
                EventName = model.EventName,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Description = model.Description,
                IsDeleted = false
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("Timeline", new { weddingId = model.WeddingId });
        }

        [HttpPost]
        public async Task<IActionResult> EditTimelineEvent(TimelineEventViewModel model)
        {
            var existing = await _context.TimelineEvents.FindAsync(model.Id);
            if (existing == null) return NotFound();

            existing.EventName = model.EventName;
            existing.StartTime = model.StartTime;
            existing.EndTime = model.EndTime;
            existing.Description = model.Description;

            _context.TimelineEvents.Update(existing);
            await _context.SaveChangesAsync();

            return RedirectToAction("Timeline", new { weddingId = existing.WeddingId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTimelineEvent(int id)
        {
            var item = await _context.TimelineEvents.FindAsync(id);
            if (item == null) return NotFound();

            item.IsDeleted = true;
            _context.TimelineEvents.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("Timeline", new { weddingId = item.WeddingId });
        }
    }
}
