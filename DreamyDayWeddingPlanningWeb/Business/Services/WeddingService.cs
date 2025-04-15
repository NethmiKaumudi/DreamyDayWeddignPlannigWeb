using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Helpers;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamyDayWeddingPlanningWeb.Business.Services
{
    public class WeddingService : IWeddingService
    {
        private readonly ApplicationDbContext _context;

        public WeddingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Wedding> GetWeddingByUserIdAsync(string userId)
        {
            return await _context.Weddings
                .FirstOrDefaultAsync(w => w.UserId == userId && !w.IsDeleted);
        }

        public async Task CreateWeddingAsync(Wedding wedding)
        {
            wedding.CreatedAt = DateTime.Now;
            wedding.IsDeleted = false;
            wedding.SpentBudget = 0;

            _context.Weddings.Add(wedding);
            await _context.SaveChangesAsync();

            // Allocate budget across categories
            var budgets = BudgetAllocationHelper.AllocateBudget(wedding.Id, wedding.TotalBudget);
            _context.Budgets.AddRange(budgets);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWeddingAsync(Wedding wedding)
        {
            _context.Weddings.Update(wedding);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWeddingAsync(int weddingId)
        {
            var wedding = await _context.Weddings.FindAsync(weddingId);
            if (wedding != null)
            {
                wedding.IsDeleted = true; // Soft delete
                await _context.SaveChangesAsync();
            }
        }

    }
}
