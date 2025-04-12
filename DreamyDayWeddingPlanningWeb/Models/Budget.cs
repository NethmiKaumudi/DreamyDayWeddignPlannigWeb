using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public int WeddingId { get; set; } // Foreign key to Wedding
        public Wedding? Wedding { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; } // e.g., Venue, Catering

        public decimal AllocatedAmount { get; set; }

        public decimal SpentAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
