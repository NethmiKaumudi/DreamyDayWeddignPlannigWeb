using DreamyDayWeddingPlanningWeb.Models;

namespace DreamyDayWeddingPlanningWeb.Business.Interfaces
{
    public interface IWeddingTimeLineService
    {
        Task<List<TimelineEvent>> GetTimelineEventsByWeddingIdAsync(int weddingId);
        Task<TimelineEvent> GetTimelineEventByIdAsync(int id, int weddingId);
        Task CreateTimelineEventAsync(TimelineEvent timelineEvent);
        Task UpdateTimelineEventAsync(TimelineEvent timelineEvent);
        Task DeleteTimelineEventAsync(int id, int weddingId);
    }
}
