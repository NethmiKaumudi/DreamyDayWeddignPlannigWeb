using System;
using System.Collections.Generic;
using DreamyDayWeddingPlanningWeb.Models;

namespace DreamyDayWeddingPlanningWeb.Models.Planner
{
    public class WeddingReportViewModel
    {
        public Wedding Wedding { get; set; }

        public string WeddingTitle => Wedding?.WeddingTitle;
        public DateTime WeddingDate => Wedding?.WeddingDate ?? DateTime.MinValue;
        public bool IsCompleted => Wedding?.IsCompleted ?? false;
        public string PlannerName { get; set; } // This should be passed from controller
        public decimal Budget { get; set; }

        public List<WeddingVendorReportItem> Vendors { get; set; } = new();
        public List<WeddingTaskReportItem> Tasks { get; set; } = new();
    }

    public class WeddingVendorReportItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; }
        public int? Priority { get; set; }
    }

    public class WeddingTaskReportItem
    {
        public string TaskName { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public string AssignedUserName { get; set; } // ✅ Needed for the report
    }
}
