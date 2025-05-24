using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models.Planner
{
    public class VendorViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public decimal Price { get; set; }

        public bool IsApproved { get; set; }

        // 🔽 Booking / Management Properties
        public bool IsDeleted { get; set; } = false;
        public bool IsAssigned { get; set; } = false;

        // Assigned wedding (from junction table)
        public int? WeddingId { get; set; }
        public string? WeddingTitle { get; set; }

        // Vendor-wedding booking notes
        public string? Notes { get; set; }

        // Booking priority (1-5)
        [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5.")]
        public int? Priority { get; set; }

        // Optional reviews placeholder
        public string? Reviews { get; set; }
    }
}
