using System.ComponentModel.DataAnnotations;

namespace DreamyDayWeddingPlanningWeb.Models
{
    public class WeddingTask
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TaskName { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public bool IsCompleted { get; set; }

        public string UserId { get; set; }
    }
}
