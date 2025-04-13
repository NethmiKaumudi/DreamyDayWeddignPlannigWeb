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
    public class GuestService : IGuestService
    {
        private readonly ApplicationDbContext _context;

        public GuestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Guest>> GetGuestsByUserIdAsync(string userId)
        {
            return await _context.Guests
                .Join(
                    _context.Weddings.Where(w => w.UserId == userId),
                    guest => guest.WeddingId,
                    wedding => wedding.Id,
                    (guest, wedding) => guest
                )
                .Where(g => !g.IsDeleted)
                .ToListAsync();
        }

        public async Task<Guest> GetGuestByIdAsync(int id, string userId)
        {
            var guest = await _context.Guests
                .Join(
                    _context.Weddings.Where(w => w.UserId == userId),
                    guest => guest.WeddingId,
                    wedding => wedding.Id,
                    (guest, wedding) => guest
                )
                .Where(g => g.Id == id && !g.IsDeleted)
                .FirstOrDefaultAsync();

            if (guest == null)
            {
                throw new KeyNotFoundException("Guest not found or you do not have access to this guest.");
            }

            return guest;
        }

        public async Task CreateGuestAsync(Guest guest)
        {
            guest.IsDeleted = false;
            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGuestAsync(Guest guest)
        {
            // Fetch the Wedding record to get the UserId
            var wedding = await _context.Weddings
                .Where(w => w.Id == guest.WeddingId)
                .FirstOrDefaultAsync();

            if (wedding == null)
            {
                throw new KeyNotFoundException("Wedding not found for the given WeddingId.");
            }

            // Fetch the guest with its Wedding navigation property
            var existingGuest = await _context.Guests
                .Where(g => g.Id == guest.Id && !g.IsDeleted)
                .Include(g => g.Wedding)
                .FirstOrDefaultAsync();

            if (existingGuest == null || existingGuest.Wedding == null || existingGuest.Wedding.UserId != wedding.UserId)
            {
                throw new KeyNotFoundException("Guest not found or you do not have access to this guest.");
            }

            existingGuest.Name = guest.Name;
            existingGuest.HasRSVPed = guest.HasRSVPed;
            existingGuest.MealPreference = guest.MealPreference;
            existingGuest.SeatingArrangement = guest.SeatingArrangement;
            existingGuest.WeddingId = guest.WeddingId;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGuestAsync(int id, string userId)
        {
            var guest = await GetGuestByIdAsync(id, userId);
            guest.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
        }
    }
}