using Newtonsoft.Json.Linq;

namespace DevProdWebApp.ViewModels
{
    public class SettingsViewModel
    {
        public string? Methodolgy { get; set; }
        public string? Preprocessing { get; set; }
        public JArray ScaleM1 { get; set; }
        public JArray ScaleM2 { get; set; }
        public JArray ScaleM3 { get; set; }
    }
}
