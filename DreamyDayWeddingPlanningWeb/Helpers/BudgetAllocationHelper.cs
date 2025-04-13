using DreamyDayWeddingPlanningWeb.Models;

namespace DreamyDayWeddingPlanningWeb.Helpers
{
    public static class BudgetAllocationHelper
    {
        public static readonly Dictionary<string, decimal> DefaultAllocations = new Dictionary<string, decimal>
        {
            { "Venue", 0.40m },        // 40%
            { "Catering", 0.20m },     // 20%
            { "Photography", 0.10m },  // 10%
            { "Decorations", 0.10m },  // 10%
            { "Entertainment", 0.10m },// 10%
            { "Attire", 0.05m },       // 5%
            { "Invitations", 0.03m },  // 3%
            { "Other", 0.02m }         // 2%
        };

        public static decimal TotalPercentage => DefaultAllocations.Values.Sum();

        public static List<Budget> AllocateBudget(int weddingId, decimal totalBudget)
        {
            if (TotalPercentage != 1m)
            {
                throw new InvalidOperationException("Total percentage of allocations must equal 100%.");
            }

            var budgets = new List<Budget>();
            foreach (var allocation in DefaultAllocations)
            {
                var budget = new Budget
                {
                    WeddingId = weddingId,
                    Category = allocation.Key,
                    AllocatedAmount = totalBudget * allocation.Value,
                    SpentAmount = 0,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                };
                budgets.Add(budget);
            }
            return budgets;
        }
    }
}