using DevProdWebApp.Models;
using DevProdWebApp.Utilities;

namespace DevProdWebApp.Repository
{
    public class ToolMetricRepo : IToolMetricRepo
    {
        private readonly AppDbContext _context;

        public ToolMetricRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ToolMetric> AddToolMetric(ToolMetric metric)
        {
            var toolMetric = await _context.ToolMetric.AddAsync(metric);
            _context.SaveChanges();
            return toolMetric.Entity;
        }

        public async Task<bool> DeleteToolMetric(int id)
        {
            var ToolMetric = await _context.ToolMetric.FindAsync(id);
            if (ToolMetric != null)
            {
                _context.ToolMetric.Remove(ToolMetric);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<ToolMetric?> GetToolMetricById(int id)
        {
            return await _context.ToolMetric.FindAsync(id);
        }

        public bool UpdateToolMetric(ToolMetric metric)
        {
            _context.ToolMetric.Update(metric);
            _context.SaveChanges();
            return true;
        }
    }
}
