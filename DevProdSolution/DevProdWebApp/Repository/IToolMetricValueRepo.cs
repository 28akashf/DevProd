using DevProdWebApp.Models;

namespace DevProdWebApp.Repository
{
    public interface IToolMetricValueRepo
    {
        Task<ToolMetricValue> GetToolMetricValueById(int id);

        bool UpdateToolMetricValue(ToolMetricValue dev);
        Task<ToolMetricValue> AddToolMetricValue(ToolMetricValue dev);
        Task<bool> DeleteToolMetricValue(int id);
        Task<bool> AddToolMetricValueList(List<ToolMetricValue> metrics);
        Task<List<ToolMetricValue>> GetToolMetricValuesByMetricId(int mid);
        Task<List<Project>> GetToolMetricValuesProjectList(int mid);
        Task<List<Developer>> GetToolMetricValuesDeveloperList(int mid);
        Task<List<ToolMetricValue>> GetFileteredToolMetricValuesByMetricId(int mid, string filter, int value);
        public  Task<bool> DeleteAllToolMetricValuesByMetricId(int mid);
    }
}
