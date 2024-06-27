using DevProdWebApp.Models;
using DevProdWebApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DevProdWebApp.Repository
{
    public class DeveloperRepo : IDeveloperRepo
    {
       
       private readonly AppDbContext _context;

        public DeveloperRepo(AppDbContext context)
        {
            _context = context;
        }
    
      public async Task<Developer> AddDeveloper(Developer dev)
        {
             var developer =  await _context.Developers.AddAsync(dev);
                _context.SaveChanges();
                return developer.Entity;
        }

        public async Task<bool> DeleteDeveloper(int id)
        {
          var developer =  await _context.Developers.FindAsync(id);
            if (developer != null)
            {
                _context.Developers.Remove(developer);
                _context.SaveChanges();
                return true; 
            }
            else 
            { 
                return false; 
            }
        }

      public async Task<List<Developer>> GetAllDevelopers()
        {
          return await _context.Developers.ToListAsync();
        }

        public async Task<Developer?> GetDeveloperById(int id)
        {
           return await _context.Developers.FindAsync(id);
        }

        public bool UpdateDeveloper(Developer dev)
        {
            _context.Developers.Update(dev);
            _context.SaveChanges();
            return true;
        }
    }
}
