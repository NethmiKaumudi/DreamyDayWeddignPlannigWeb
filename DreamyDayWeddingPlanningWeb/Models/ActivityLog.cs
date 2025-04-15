using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Foreign key to AspNetUsers
        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(200)]
        public string Action { get; set; } // e.g., "User Login", "Vendor Approved"

        public DateTime Timestamp { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string Details { get; set; }
    }
}
