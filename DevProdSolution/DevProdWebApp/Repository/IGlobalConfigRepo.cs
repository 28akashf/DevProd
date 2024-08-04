using DevProdWebApp.Models;

namespace DevProdWebApp.Repository
{
    public interface IGlobalConfigRepo
    {
        Task<int> GetCurrentSettingId();
    }
}
