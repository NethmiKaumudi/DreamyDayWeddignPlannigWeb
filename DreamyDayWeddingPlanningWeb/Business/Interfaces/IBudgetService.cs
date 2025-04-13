using DreamyDayWeddingPlanningWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamyDayWeddingPlanningWeb.Business.Interfaces
{
    public interface IBudgetService
    {
        Task<List<Budget>> GetBudgetsByUserIdAsync(string userId);
        Task<Budget> GetBudgetByIdAsync(int id, string userId);
        Task CreateBudgetAsync(Budget budget);
        Task UpdateBudgetAsync(Budget budget);
        Task DeleteBudgetAsync(int id, string userId);
        Task<(decimal TotalAllocated, decimal TotalSpent)> CalculateBudgetTotalsAsync(int weddingId);
        Task<bool> CheckBudgetExceededAsync(int weddingId);

    }
}