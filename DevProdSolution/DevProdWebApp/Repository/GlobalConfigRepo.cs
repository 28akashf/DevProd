
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
    }
}
