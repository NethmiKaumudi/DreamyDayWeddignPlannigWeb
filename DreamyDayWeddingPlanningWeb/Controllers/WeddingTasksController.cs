using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DreamyDayWeddingPlanningWeb.Controllers
{
    [Authorize(Roles = "Couple")]
    public class WeddingTasksController : Controller
    {
        private readonly IWeddingTaskService _weddingTaskService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IValidator<WeddingTask> _validator;
        private readonly ApplicationDbContext _context;

        public WeddingTasksController(
            IWeddingTaskService weddingTaskService,
            UserManager<ApplicationUser> userManager,
            IValidator<WeddingTask> validator,
            ApplicationDbContext context)
        {
            _weddingTaskService = weddingTaskService;
            _userManager = userManager;
            _validator = validator;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var tasks = await _weddingTaskService.GetTasksByUserIdAsync(userId);
                return View(tasks);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                TempData["ErrorMessage"] = "Task ID cannot be null or invalid.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                var weddingTask = await _weddingTaskService.GetTaskByIdAsync(id.Value, userId);
                return View(weddingTask);
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskName,Deadline,IsCompleted")] WeddingTask weddingTask)
        {
            try
            {
                weddingTask.UserId = _userManager.GetUserId(User);
                var validationResult = await _validator.ValidateAsync(weddingTask);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return View(weddingTask);
                }

                await _weddingTaskService.CreateTaskAsync(weddingTask);
                TempData["SuccessMessage"] = "Task created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(weddingTask);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                TempData["ErrorMessage"] = "Task ID cannot be null or invalid.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                var weddingTask = await _weddingTaskService.GetTaskByIdAsync(id.Value, userId);
                return View(weddingTask);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskName,Deadline,IsCompleted")] WeddingTask weddingTask)
        {
            if (id != weddingTask.Id)
            {
                TempData["ErrorMessage"] = "Task ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var validationResult = await _validator.ValidateAsync(weddingTask);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return View(weddingTask);
                }

                await _weddingTaskService.UpdateTaskAsync(weddingTask);
                TempData["SuccessMessage"] = "Task updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(weddingTask);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0)
            {
                TempData["ErrorMessage"] = "Task ID cannot be null or invalid.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                var weddingTask = await _weddingTaskService.GetTaskByIdAsync(id.Value, userId);
                return View(weddingTask);
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
                await _weddingTaskService.DeleteTaskAsync(id, userId);
                TempData["SuccessMessage"] = "Task soft-deleted successfully.";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Json(new { success = false, message = "Task ID must be a positive integer." });
                }

                var userId = _userManager.GetUserId(User);
                var task = await _weddingTaskService.GetTaskByIdAsync(id, userId);
                if (task == null)
                {
                    return Json(new { success = false, message = "Task not found." });
                }

                if (task.IsCompleted)
                {
                    return Json(new { success = false, message = "Task is already completed." });
                }

                task.IsCompleted = true;
                await _weddingTaskService.UpdateTaskAsync(task);

                var wedding = await _context.Weddings
                    .Where(w => w.UserId == userId && !w.IsDeleted)
                    .FirstOrDefaultAsync();
                if (wedding != null)
                {
                    var progress = await _weddingTaskService.CalculateTaskProgressAsync(userId);
                    wedding.Progress = (int)Math.Round(progress);
                    _context.Update(wedding);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}