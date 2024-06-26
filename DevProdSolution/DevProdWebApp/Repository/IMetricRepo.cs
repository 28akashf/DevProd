using DevProdWebApp.Models;

namespace DevProdWebApp.Repository
{
    public interface IMetricRepo
    {
        Task<List<Metric>> GetAllMetrics();
        Task<Metric?> GetMetricById(int id);
        Task<bool> AddMetric(Metric dev);

        bool UpdateMetric(Metric dev);
        Task<bool> DeleteMetric(int id);
    }
}
