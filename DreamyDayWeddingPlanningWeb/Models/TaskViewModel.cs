using System;

namespace DreamyDayWeddingPlanningWeb.Models.Planner
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }

        public int WeddingId { get; set; }
        public string WeddingName { get; set; } = string.Empty;

        public string AssignedToUserId { get; set; } = string.Empty;
        public string AssignedUserName { get; set; } = string.Empty;
    }
}
