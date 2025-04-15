using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamyDayWeddingPlanningWeb.Business.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly ApplicationDbContext _context;

        public BudgetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Budget>> GetBudgetsByUserIdAsync(string userId)
        {
            return await _context.Budgets
                .Where(b => b.Wedding.UserId == userId && !b.IsDeleted)
                .Include(b => b.Wedding)
                .ToListAsync();
        }

        public async Task<Budget> GetBudgetByIdAsync(int id, string userId)
        {
            var budget = await _context.Budgets
                .Where(b => b.Id == id && b.Wedding.UserId == userId && !b.IsDeleted)
                .Include(b => b.Wedding)
                .FirstOrDefaultAsync();

            if (budget == null)
            {
                throw new KeyNotFoundException("Budget item not found or you do not have access to this budget item.");
            }

            return budget;
        }

        public async Task CreateBudgetAsync(Budget budget)
        {
            // Check if the category already exists for this wedding
            var existingBudget = await _context.Budgets
                .Where(b => b.WeddingId == budget.WeddingId && b.Category == budget.Category && !b.IsDeleted)
                .FirstOrDefaultAsync();

            if (existingBudget != null)
            {
                throw new InvalidOperationException($"A budget item for category '{budget.Category}' already exists for this wedding.");
            }

            budget.IsDeleted = false;
            budget.CreatedAt = DateTime.Now;
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();

            await ValidateAndUpdateWeddingBudget(budget.WeddingId);
        }

        public async Task UpdateBudgetAsync(Budget budget)
        {
            var existingBudget = await _context.Budgets
                .Where(b => b.Id == budget.Id && !b.IsDeleted)
                .Include(b => b.Wedding)
                .FirstOrDefaultAsync();

            if (existingBudget == null)
            {
                throw new KeyNotFoundException("Budget item not found or you do not have access to this budget item.");
            }

            // Check if the updated category already exists (excluding the current budget item)
            var duplicateBudget = await _context.Budgets
                .Where(b => b.WeddingId == budget.WeddingId && b.Category == budget.Category && b.Id != budget.Id && !b.IsDeleted)
                .FirstOrDefaultAsync();

            if (duplicateBudget != null)
            {
                throw new InvalidOperationException($"A budget item for category '{budget.Category}' already exists for this wedding.");
            }

            existingBudget.Category = budget.Category;
            existingBudget.AllocatedAmount = budget.AllocatedAmount;
            existingBudget.SpentAmount = budget.SpentAmount;
            await _context.SaveChangesAsync();

            await ValidateAndUpdateWeddingBudget(existingBudget.WeddingId);
        }

        public async Task DeleteBudgetAsync(int id, string userId)
        {
            var budget = await GetBudgetByIdAsync(id, userId);
            budget.IsDeleted = true;
            await _context.SaveChangesAsync();

            await ValidateAndUpdateWeddingBudget(budget.WeddingId);
        }

        public async Task<(decimal TotalAllocated, decimal TotalSpent)> CalculateBudgetTotalsAsync(int weddingId)
        {
            var budgets = await _context.Budgets
                .Where(b => b.WeddingId == weddingId && !b.IsDeleted)
                .ToListAsync();

            decimal totalAllocated = budgets.Sum(b => b.AllocatedAmount);
            decimal totalSpent = budgets.Sum(b => b.SpentAmount);

            return (totalAllocated, totalSpent);
        }

        public async Task<bool> CheckBudgetExceededAsync(int weddingId)
        {
            var wedding = await _context.Weddings
                .Where(w => w.Id == weddingId && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                throw new KeyNotFoundException("Wedding not found.");
            }

            var (totalAllocated, totalSpent) = await CalculateBudgetTotalsAsync(weddingId);
            return totalSpent > wedding.TotalBudget;
        }

        private async Task ValidateAndUpdateWeddingBudget(int weddingId)
        {
            var wedding = await _context.Weddings
                .Where(w => w.Id == weddingId && !w.IsDeleted)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                throw new KeyNotFoundException("Wedding not found.");
            }

            var (totalAllocated, totalSpent) = await CalculateBudgetTotalsAsync(weddingId);

            if (totalAllocated > wedding.TotalBudget)
            {
                throw new InvalidOperationException("Total allocated budget exceeds the wedding's total budget.");
            }

            wedding.SpentBudget = totalSpent;
            await _context.SaveChangesAsync();
        }
    }
}