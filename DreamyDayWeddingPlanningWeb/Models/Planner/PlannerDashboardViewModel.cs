namespace DreamyDayWeddingPlanningWeb.Models.Planner
{
    public class PlannerDashboardViewModel
    {
        public int TotalWeddings { get; set; }
        public int TasksCompleted { get; set; }
        public int TasksTotal { get; set; }

        public List<WeddingSummaryViewModel> UpcomingWeddings { get; set; } = new();
        public List<WeddingSummaryViewModel> AllWeddings { get; set; } = new();
        public List<TaskViewModel> UpcomingTasks { get; set; } = new();
        public List<NotificationViewModel> Notifications { get; set; } = new();
    }

    public class WeddingSummaryViewModel
    {
        public int WeddingId { get; set; }
        public string CoupleName { get; set; }
        public DateTime WeddingDate { get; set; }
        public bool IsCompleted { get; set; }
        public decimal Budget { get; set; } // ← NEW
    }


    public class NotificationViewModel
    {
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
