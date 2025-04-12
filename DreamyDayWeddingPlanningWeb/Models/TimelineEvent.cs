using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models
{
    public class TimelineEvent
    {
        public int Id { get; set; }

        [Required]
        public int WeddingId { get; set; } // Foreign key to Wedding
        public Wedding? Wedding { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
