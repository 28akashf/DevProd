using DevProdWebApp.Models;
using DevProdWebApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DevProdWebApp.Repository
{
    public class ProjectRepo : IProjectRepo
    {
        private readonly AppDbContext _context;

        public ProjectRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProject(Project dev)
        {
            await _context.Projects.AddAsync(dev);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteProject(int id)
        {
            var Project = await _context.Projects.FindAsync(id);
            if (Project != null)
            {
                _context.Projects.Remove(Project);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project?> GetProjectById(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public bool UpdateProject(Project dev)
        {
            _context.Projects.Update(dev);
            _context.SaveChanges();
            return true;
        }
        public async Task<Project?> GetProjectByProjectName(string pname)
        {
            return await _context.Projects.FirstOrDefaultAsync(x => x.Name == pname);
        }
    }
}
