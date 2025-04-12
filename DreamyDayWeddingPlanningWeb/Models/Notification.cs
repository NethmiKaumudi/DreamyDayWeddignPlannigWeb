using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Foreign key to AspNetUsers
        public ApplicationUser User { get; set; }


        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
