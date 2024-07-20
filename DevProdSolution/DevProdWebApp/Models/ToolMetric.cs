using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevProdWebApp.Models
{
    public class ToolMetric
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Weight { get; set; }
        public string? Scale { get; set; }

        public List<ToolMetricValue>? ToolMetricValues { get; set; }
        [ForeignKey("Setting")]
        public int SettingId { get; set; }
        public Setting Setting { get; set; }

    }
}
