using DevProdWebApp.Models;
using DevProdWebApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DevProdWebApp.Repository
{
    public class SettingsRepo : ISettingsRepo
    {
        private readonly AppDbContext _context;

        public SettingsRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Setting> AddSettings(Setting dev)
        {
            var Setting = await _context.Settings.AddAsync(dev);
            _context.SaveChanges();
            return Setting.Entity;
        }

        public async Task<bool> DeleteSettings(int id)
        {
            var Setting = await _context.Settings.FindAsync(id);
            if (Setting != null)
            {
                _context.Settings.Remove(Setting);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<Setting?> GetSettingsById(int id)
        {
            return await _context.Settings.FindAsync(id);
        }

        public async Task<Setting?> GetSettingsWithMetricsById(int id)
        {
            return await _context.Settings.Include(x => x.ToolMetricList).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public bool UpdateSettings(Setting dev)
        {
            _context.Settings.Update(dev);
            _context.SaveChanges();
            return true;
        }
    }
}
