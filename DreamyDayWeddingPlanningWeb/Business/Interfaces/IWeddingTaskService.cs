using DreamyDayWeddingPlanningWeb.Models;

namespace DreamyDayWeddingPlanningWeb.Business.Interfaces
{
    public interface IWeddingTaskService
    {
        Task<List<WeddingTask>> GetTasksByUserIdAsync(string userId);
        Task<WeddingTask> GetTaskByIdAsync(int id, string userId);
        Task CreateTaskAsync(WeddingTask weddingTask);
        Task UpdateTaskAsync(WeddingTask weddingTask);
        Task DeleteTaskAsync(int id, string userId);
        Task<double> CalculateTaskProgressAsync(string userId);
    }
}
