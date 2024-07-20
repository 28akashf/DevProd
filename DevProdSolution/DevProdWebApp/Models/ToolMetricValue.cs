using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevProdWebApp.Models
{
    public class ToolMetricValue
    {
        [Key]
        public int Id { get; set; }
        public string? Value { get; set; }

        [ForeignKey("ToolMetric")]
        public int ToolMetricId { get; set; }
        public ToolMetric ToolMetric { get; set; }
    }
}
