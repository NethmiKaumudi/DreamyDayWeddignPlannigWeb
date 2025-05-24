using System.Threading.Tasks;

namespace DreamyDayWeddingPlanningWeb.Services
{
    public interface IViewRenderService

    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
