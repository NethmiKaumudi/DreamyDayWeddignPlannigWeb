namespace DreamyDayWeddingPlanningWeb.Models.Planner
{
	public class TaskViewModel
	{
		public int Id { get; set; }
		public string TaskName { get; set; }
		public DateTime Deadline { get; set; }
		public bool IsCompleted { get; set; }
	}
}
