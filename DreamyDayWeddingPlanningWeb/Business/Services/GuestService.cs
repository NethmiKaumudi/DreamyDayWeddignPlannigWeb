using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

                // Find the user's wedding
                var wedding = await _context.Weddings
                    .FirstOrDefaultAsync(w => w.UserId == userId && !w.IsDeleted);

                if (wedding == null)
                    return new List<Guest>();

                return await _context.Guests
                    .Include(g => g.Wedding)
                    .Where(g => g.WeddingId == wedding.Id && !g.IsDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving guests.", ex);
            }
        }

        public async Task<Guest> GetGuestByIdAsync(int id, string userId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid guest ID.", nameof(id));
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

                // Find the user's wedding
                var wedding = await _context.Weddings
                    .FirstOrDefaultAsync(w => w.UserId == userId && !w.IsDeleted);

                if (wedding == null)
                    throw new KeyNotFoundException("No wedding found for the user.");

                var guest = await _context.Guests
                    .Include(g => g.Wedding)
                    .FirstOrDefaultAsync(g => g.Id == id && g.WeddingId == wedding.Id && !g.IsDeleted);

                if (guest == null)
                    throw new KeyNotFoundException($"Guest with ID {id} not found for the user.");

                return guest;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving guest with ID {id}.", ex);
            }
        }

        public async Task CreateGuestAsync(Guest guest)
        {
            try
            {
                if (guest == null)
                    throw new ArgumentNullException(nameof(guest), "Guest cannot be null.");

                _context.Guests.Add(guest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while creating the guest. Please check the data and try again.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while creating the guest.", ex);
            }
        }

        public async Task UpdateGuestAsync(Guest guest)
        {
            try
            {
                if (guest == null)
                    throw new ArgumentNullException(nameof(guest), "Guest cannot be null.");

                var existingGuest = await _context.Guests.FindAsync(guest.Id);
                if (existingGuest == null)
                    throw new KeyNotFoundException($"Guest with ID {guest.Id} not found.");

                // Update properties
                existingGuest.Name = guest.Name;
                existingGuest.HasRSVPed = guest.HasRSVPed;
                existingGuest.MealPreference = guest.MealPreference;
                existingGuest.SeatingArrangement = guest.SeatingArrangement;
                // WeddingId should not be updated by the user
                // IsDeleted should not be updated directly

                _context.Guests.Update(existingGuest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("The guest was modified by another user. Please refresh and try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the guest. Please check the data and try again.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the guest.", ex);
            }
        }

        public async Task SoftDeleteGuestAsync(int id, string userId)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid guest ID.", nameof(id));
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

                // Find the user's wedding
                var wedding = await _context.Weddings
                    .FirstOrDefaultAsync(w => w.UserId == userId && !w.IsDeleted);

                if (wedding == null)
                    throw new KeyNotFoundException("No wedding found for the user.");

                var guest = await _context.Guests
                    .FirstOrDefaultAsync(g => g.Id == id && g.WeddingId == wedding.Id && !g.IsDeleted);

                if (guest == null)
                    throw new KeyNotFoundException($"Guest with ID {id} not found for the user.");

                guest.IsDeleted = true;
                _context.Guests.Update(guest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the guest.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred while deleting guest with ID {id}.", ex);
            }
        }
    }
}
