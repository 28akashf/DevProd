using DevProdWebApp.Models;

namespace DevProdWebApp.Repository
{
    public interface IGlobalConfigRepo
    {
        Task<int> GetCurrentSettingId();
        Task<bool> ChangeSettingId(string id);
    }
}
