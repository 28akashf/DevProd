using DevProdWebApp.Models;

namespace DevProdWebApp.Repository
{
    public interface IDeveloperRepo
    {
        Task<List<Developer>> GetAllDevelopers();
        Task<Developer?> GetDeveloperById(int id);
        Task<Developer> AddDeveloper(Developer dev);

        bool UpdateDeveloper(Developer dev);
        Task<bool> DeleteDeveloper(int id);
    }
}
