using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models
{
    public class Vendor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } // e.g., Venue, Caterer

        [StringLength(500)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsApproved { get; set; } // For Admin approval

        [StringLength(200)]
        public string Reviews { get; set; } // JSON or comma-separated reviews

        //public int? WeddingId { get; set; }
        //public Wedding? Wedding { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
