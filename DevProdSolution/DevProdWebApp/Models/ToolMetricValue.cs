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

        [ForeignKey("Developer")]
        public int? DeveloperId { get; set; }
        public Developer? Developer { get; set; }

        [ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }


    }
}
