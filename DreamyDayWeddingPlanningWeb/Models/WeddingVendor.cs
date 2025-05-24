using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamyDayWeddingPlanningWeb.Models
{
    public class WeddingVendor
    {
        [Key]
        public int Id { get; set; }

        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public string? Notes { get; set; }

        public int? Priority { get; set; }

        public string? AssignedByPlannerId { get; set; }
        public ApplicationUser? AssignedByPlanner { get; set; }
    }
}
