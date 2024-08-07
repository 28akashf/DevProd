
using DevProdWebApp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace DevProdWebApp.Repository
{
    public class GlobalConfigRepo : IGlobalConfigRepo
    {
        private readonly AppDbContext _context;

        public GlobalConfigRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetCurrentSettingId()
        {
          var setting = await  _context.GlobalConfigs.FirstOrDefaultAsync(x => x.ConfigKey == "CurrentSetting");
            return int.Parse(setting.ConfigValue);
        }
        public async Task<bool> ChangeSettingId(string id)
        {
            var setting = await _context.GlobalConfigs.FirstOrDefaultAsync(x => x.ConfigKey == "CurrentSetting");
            setting.ConfigValue = id;
            _context.GlobalConfigs.Update(setting);
            _context.SaveChanges();
            return true;
        }
    }
}
