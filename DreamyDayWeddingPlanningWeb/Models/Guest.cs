using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models
{
    public class Guest
    {
        public int Id { get; set; }

        [Required]
        public int WeddingId { get; set; } // Foreign key to Wedding
        public Wedding? Wedding { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool HasRSVPed { get; set; }

        [StringLength(50)]
        public string MealPreference { get; set; }

        [StringLength(50)]
        public string SeatingArrangement { get; set; }
        public bool IsDeleted { get; set; } = false; // Soft delete flag
    }
}
