using DevProdWebApp.Models;

namespace DevProdWebApp.Repository
{
    public interface IToolMetricRepo
    {
        Task<ToolMetric> GetToolMetricById(int id);

        bool UpdateToolMetric(ToolMetric dev);
        Task<ToolMetric> AddToolMetric(ToolMetric dev);
        Task<bool> DeleteToolMetric(int id);
    }
}
