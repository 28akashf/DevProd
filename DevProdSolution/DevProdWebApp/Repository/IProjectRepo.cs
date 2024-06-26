using DevProdWebApp.Models;

namespace DevProdWebApp.Repository
{
    public interface IProjectRepo
    {
        Task<List<Project>> GetAllProjects();
        Task<Project?> GetProjectById(int id);
        Task<bool> AddProject(Project dev);

        bool UpdateProject(Project dev);
        Task<bool> DeleteProject(int id);
    }
}
