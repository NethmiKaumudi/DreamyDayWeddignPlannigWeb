// Controllers/CoupleController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamyDayWeddingPlanningWeb.Controllers
{
    [Authorize(Roles = "Couple")] // Restrict to Couple role
    public class CoupleController : Controller
    {
        public IActionResult Dashboard()
        {
            return View("Dashboard","Couple");
        }

        public IActionResult Overview()
        {
            return View();
        }

        public IActionResult Notifications()
        {
            return View();
        }

        public IActionResult Checklist()
        {
            return RedirectToAction("Index", "WeddingTasks");
        }

        public IActionResult GuestList()
        {
            return View();
        }

        public IActionResult Budget()
        {
            return View();
        }

        public IActionResult Timeline()
        {
            return View();
        }

        public IActionResult Vendors()
        {
            return View();
        }

        public IActionResult PlannerCommunication()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}