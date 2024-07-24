using System.ComponentModel.DataAnnotations;

namespace DevProdWebApp.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string? Methodolgy { get; set; }
        public string? Preprocessing { get; set; }
        public string? Grouping { get; set; }
        public string? SubGrouping { get; set; }
        public string? Parameters { get; set; }
        public string? Scale { get; set; }

        public List<ToolMetric>? ToolMetricList { get; set; }

    }
}
