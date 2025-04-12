using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using DreamyDayWeddingPlanningWeb.Business.Interfaces;

namespace DreamyDayWeddingPlanningWeb.Controllers
{
    [Authorize(Roles = "Couple")]
    public class WeddingTasksController : Controller
    {
        private readonly IWeddingTaskService _weddingTaskService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IValidator<WeddingTask> _validator;

        public WeddingTasksController(IWeddingTaskService weddingTaskService, UserManager<ApplicationUser> userManager, IValidator<WeddingTask> validator)
        {
            _weddingTaskService = weddingTaskService;
            _userManager = userManager;
            _validator = validator;
        }

        // GET: WeddingTasks
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

        // GET: WeddingTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Task ID cannot be null.";
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

        // GET: WeddingTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WeddingTasks/Create
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

        // GET: WeddingTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Task ID cannot be null.";
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

        // POST: WeddingTasks/Edit/5
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

        // GET: WeddingTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Task ID cannot be null.";
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

        // POST: WeddingTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                await _weddingTaskService.DeleteTaskAsync(id, userId);
                TempData["SuccessMessage"] = "Task deleted successfully.";
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