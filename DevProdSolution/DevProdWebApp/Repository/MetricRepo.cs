using DevProdWebApp.Models;
using DevProdWebApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DevProdWebApp.Repository
{
    public class MetricRepo : IMetricRepo
    {
        private readonly AppDbContext _context;

        public MetricRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMetric(Metric dev)
        {
            await _context.Metrics.AddAsync(dev);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteMetric(int id)
        {
            var Metric = await _context.Metrics.FindAsync(id);
            if (Metric != null)
            {
                _context.Metrics.Remove(Metric);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Metric>> GetAllMetrics()
        {
            return await _context.Metrics.ToListAsync();
        }

        public async Task<Metric?> GetMetricById(int id)
        {
            return await _context.Metrics.FindAsync(id);
        }

        public bool UpdateMetric(Metric dev)
        {
            _context.Metrics.Update(dev);
            _context.SaveChanges();
            return true;
        }

    }
}
