using DevProdWebApp.Models;
using Newtonsoft.Json.Linq;

namespace DevProdWebApp.ViewModels
{
    public class SettingsViewModel
    {
        public string? Methodolgy { get; set; }
        public string? Preprocessing { get; set; }
        public string? Grouping { get; set; }
        public string? SubGrouping { get; set; }
        public string? Parameters { get; set; }
        public List<ToolMetric> ToolMetricList { get; set; }
        public  List<MetricScale> ToolMetricScaleList { get; set; }
        public  List<Developer> DeveloperList { get; set; }
        public  List<Project> ProjectList { get; set; }
    }

public class  ScaleObject
{
    public string Description { get; set; }
    public string LowerBound { get; set; }
    public string UpperBound { get; set; }
}
public class MetricScale
{
    public string MetricName { get; set; }
    public List<ScaleObject> ScaleObjects { get; set; }
}

}
