using DreamyDayWeddingPlanningWeb.Models;

namespace DreamyDayWeddingPlanningWeb.Business.Interfaces
{
    public interface IGuestService
    {
        Task<List<Guest>> GetGuestsByUserIdAsync(string userId);
        Task<Guest> GetGuestByIdAsync(int id, string userId);
        Task CreateGuestAsync(Guest guest);
        Task UpdateGuestAsync(Guest guest);
        Task SoftDeleteGuestAsync(int id, string userId);
    }
}
