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

        public async Task<bool> DeleteAllToolMetricValuesByMetricId(int mid)
        {
            var ToolMetricValue = await _context.ToolMetricValues.Where(x=>x.ToolMetricId==mid).ToListAsync();
            if (ToolMetricValue != null)
            {
                foreach (var item in ToolMetricValue)
                {
                    _context.ToolMetricValues.Remove(item);
                
                }
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
        public async Task<List<ToolMetricValue>> GetFileteredToolMetricValuesByMetricId(int mid,string filter,int value)
        {
            List<ToolMetricValue> result = null;
            switch(filter)
            {
                case "developer":
                    result = await _context.ToolMetricValues.Where(x => x.ToolMetricId == mid && x.DeveloperId == value).ToListAsync();
                    break;
                case "project":
               result = await _context.ToolMetricValues.Where(x => x.ToolMetricId == mid && x.ProjectId == value).ToListAsync();
                    break;
                case "date":
                    result = await _context.ToolMetricValues.Where(x => x.ToolMetricId == mid && x.TimeStamp >= DateTime.Now.AddDays((0-value))).ToListAsync();
                    break;
                default:
               result =  await _context.ToolMetricValues.Where(x => x.ToolMetricId == mid).ToListAsync();
                    break;
            }
             


            return result;
        }

        public async Task<List<Developer>> GetToolMetricValuesDeveloperList(int mid)
        {
            return await _context.ToolMetricValues.Include(x=>x.Developer).Where(x => x.ToolMetricId == mid).Select(x=>x.Developer).Distinct().ToListAsync();
        }
        public async Task<List<Project>> GetToolMetricValuesProjectList(int mid)
        {
            return await _context.ToolMetricValues.Include(x => x.Developer).Where(x => x.ToolMetricId == mid).Select(x => x.Project).Distinct().ToListAsync();
        }
        public bool UpdateToolMetricValue(ToolMetricValue metric)
        {
            _context.ToolMetricValues.Update(metric);
            _context.SaveChanges();
            return true;
        }
    }
}
