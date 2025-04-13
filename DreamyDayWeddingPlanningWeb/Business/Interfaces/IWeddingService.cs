using DreamyDayWeddingPlanningWeb.Models;

namespace DreamyDayWeddingPlanningWeb.Business.Interfaces
{
    public interface IWeddingService
    {
        Task<Wedding> GetWeddingByUserIdAsync(string userId);
        Task CreateWeddingAsync(Wedding wedding);
        Task UpdateWeddingAsync(Wedding wedding);
        Task DeleteWeddingAsync(int weddingId);
    }
}
