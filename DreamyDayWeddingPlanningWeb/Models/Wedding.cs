using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
namespace DreamyDayWeddingPlanningWeb.Models
{
    public class Wedding
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Foreign key to AspNetUsers (Couple)
        public ApplicationUser User { get; set; }

        public string PlannerId { get; set; } // Foreign key to AspNetUsers (Planner, nullable)
        public ApplicationUser? Planner { get; set; }

        [Required]
        public DateTime WeddingDate { get; set; }

        public decimal TotalBudget { get; set; }

        public decimal SpentBudget { get; set; }

        public double Progress { get; set; } // Percentage (e.g., 60.5)

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
