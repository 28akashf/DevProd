using DevProdWebApp.Models;

namespace DevProdWebApp.Repository
{
    public interface ISettingsRepo
    {
        Task<Setting> GetSettingsById(int id);
        Task<Setting?> GetSettingsWithMetricsById(int id);

        bool UpdateSettings(Setting dev);
        Task<Setting> AddSettings(Setting dev);
        Task<bool> DeleteSettings(int id);
    }
}
