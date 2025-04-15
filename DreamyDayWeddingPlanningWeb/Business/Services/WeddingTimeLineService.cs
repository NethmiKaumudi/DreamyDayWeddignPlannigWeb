using DreamyDayWeddingPlanningWeb.Business.Interfaces;
using DreamyDayWeddingPlanningWeb.Data;
using DreamyDayWeddingPlanningWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace DreamyDayWeddingPlanningWeb.Business.Services
{
    public class WeddingTimeLineService : IWeddingTimeLineService
    {
        private readonly ApplicationDbContext _context;

        public WeddingTimeLineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TimelineEvent>> GetTimelineEventsByWeddingIdAsync(int weddingId)
        {
            return await _context.TimelineEvents
                .Where(te => te.WeddingId == weddingId && !te.IsDeleted)
                .OrderBy(te => te.StartTime)
                .Include(te => te.Wedding)
                .ToListAsync();
        }

        public async Task<TimelineEvent> GetTimelineEventByIdAsync(int id, int weddingId)
        {
            return await _context.TimelineEvents
                .Where(te => te.Id == id && te.WeddingId == weddingId && !te.IsDeleted)
                .Include(te => te.Wedding)
                .FirstOrDefaultAsync();
        }

        public async Task CreateTimelineEventAsync(TimelineEvent timelineEvent)
        {
            timelineEvent.IsDeleted = false;
            _context.TimelineEvents.Add(timelineEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTimelineEventAsync(TimelineEvent timelineEvent)
        {
            _context.TimelineEvents.Update(timelineEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTimelineEventAsync(int id, int weddingId)
        {
            var timelineEvent = await _context.TimelineEvents
                .Where(te => te.Id == id && te.WeddingId == weddingId && !te.IsDeleted)
                .FirstOrDefaultAsync();

            if (timelineEvent != null)
            {
                timelineEvent.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

    }
}
