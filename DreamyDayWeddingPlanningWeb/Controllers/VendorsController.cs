using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DreamyDayWeddingPlanningWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamyDayWeddingPlanningWeb.Controllers
{
    [Authorize(Roles = "Couple")]
    public class VendorsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public VendorsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Ensure the user is authenticated
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Hardcoded list of vendors (simulating a database)
            var vendors = new List<Vendor>
            {
                // Venues
                new Vendor { Id = 1, Name = "Sunset Manor", Category = "Venues", Price = 5000.00m, Description = "Elegant venue with outdoor garden and indoor ballroom, accommodates up to 200 guests.", Reviews = "4.8/5 - Beautiful setting, great staff!", IsBooked = false },
                new Vendor { Id = 2, Name = "Crystal Pavilion", Category = "Venues", Price = 7500.00m, Description = "Modern venue with stunning views, perfect for large weddings up to 300 guests.", Reviews = "4.9/5 - Amazing ambiance!", IsBooked = false },
                new Vendor { Id = 3, Name = "Rustic Barn Retreat", Category = "Venues", Price = 3000.00m, Description = "Charming rustic barn with open fields, ideal for intimate weddings up to 100 guests.", Reviews = "4.7/5 - Cozy and unique!", IsBooked = false },

                // Photographers
                new Vendor { Id = 4, Name = "Moments by Mia", Category = "Photographers", Price = 2000.00m, Description = "Specializes in candid and artistic wedding photography, full-day coverage.", Reviews = "4.6/5 - Captured every moment perfectly!", IsBooked = false },
                new Vendor { Id = 5, Name = "Lens of Love", Category = "Photographers", Price = 2500.00m, Description = "Professional wedding photography with a focus on romantic portraits.", Reviews = "4.8/5 - Stunning photos!", IsBooked = false },
                new Vendor { Id = 6, Name = "Click & Cherish", Category = "Photographers", Price = 1500.00m, Description = "Budget-friendly photography with half-day coverage and digital album.", Reviews = "4.5/5 - Great value!", IsBooked = false },

                // Florists
                new Vendor { Id = 7, Name = "Bloom Bliss", Category = "Florists", Price = 1000.00m, Description = "Custom floral arrangements for ceremonies and receptions, seasonal blooms.", Reviews = "4.7/5 - Gorgeous flowers!", IsBooked = false },
                new Vendor { Id = 8, Name = "Petal Perfection", Category = "Florists", Price = 2000.00m, Description = "Luxury floral designs, specializing in large installations.", Reviews = "4.9/5 - Absolutely stunning!", IsBooked = false },
                new Vendor { Id = 9, Name = "Wildflower Wonders", Category = "Florists", Price = 800.00m, Description = "Eco-friendly floral arrangements with a natural, rustic vibe.", Reviews = "4.6/5 - Loved the aesthetic!", IsBooked = false },

                // Caterers
                new Vendor { Id = 10, Name = "Gourmet Gala", Category = "Caterers", Price = 4000.00m, Description = "Full-service catering with customizable menus, serves up to 200 guests.", Reviews = "4.8/5 - Delicious food, great service!", IsBooked = false },
                new Vendor { Id = 11, Name = "Taste of Elegance", Category = "Caterers", Price = 6000.00m, Description = "Premium catering with fine dining experience, includes staff.", Reviews = "4.9/5 - Exquisite flavors!", IsBooked = false },
                new Vendor { Id = 12, Name = "Savory Soirée", Category = "Caterers", Price = 2500.00m, Description = "Affordable catering with buffet-style service, great for small weddings.", Reviews = "4.5/5 - Tasty and reliable!", IsBooked = false }
            };

            // Pass the vendors to the view
            return View(vendors);
        }
    }
}