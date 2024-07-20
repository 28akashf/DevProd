using DevProdWebApp.Models;
using DevProdWebApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DevProdWebApp.Repository
{
    public class ToolMetricValueRepo : IToolMetricValueRepo
    {
        private readonly AppDbContext _context;

        public ToolMetricValueRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ToolMetricValue> AddToolMetricValue(ToolMetricValue metric)
        {
            var ToolMetricValue = await _context.ToolMetricValues.AddAsync(metric);
            _context.SaveChanges();
            return ToolMetricValue.Entity;
        }

        public async Task<bool> AddToolMetricValueList(List<ToolMetricValue> metrics)
        {
             await _context.ToolMetricValues.AddRangeAsync(metrics);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteToolMetricValue(int id)
        {
            var ToolMetricValue = await _context.ToolMetricValues.FindAsync(id);
            if (ToolMetricValue != null)
            {
                _context.ToolMetricValues.Remove(ToolMetricValue);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<ToolMetricValue?> GetToolMetricValueById(int id)
        {
            return await _context.ToolMetricValues.FindAsync(id);
        }

        public async Task<List<ToolMetricValue>> GetToolMetricValuesByMetricId(int mid)
        {
            return await _context.ToolMetricValues.Where(x=>x.ToolMetricId==mid).ToListAsync();
        }

        public bool UpdateToolMetricValue(ToolMetricValue metric)
        {
            _context.ToolMetricValues.Update(metric);
            _context.SaveChanges();
            return true;
        }
    }
}
