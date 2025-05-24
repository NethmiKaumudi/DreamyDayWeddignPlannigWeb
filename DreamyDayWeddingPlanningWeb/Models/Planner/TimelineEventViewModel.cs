using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models.Planner
{
    public class TimelineEventViewModel
    {
        public int Id { get; set; }

        [Required]
        public int WeddingId { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string? Description { get; set; }
    }
}
