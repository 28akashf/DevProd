using DevProdWebApp.Models;

namespace DevProdWebApp.ViewModels
{
    public class Metric
    {
       public int Id { get; set; }
        public string? Name { get; set; }
        public double Weight  { get; set; }
    }

    public class MetricList
    {
        public int Id { get; set; }
        public string Project { get; set; }
        public List<Metric> MetricSet { get; set; }

       

    }

    public class Result
    {
          public object Data { get; set; }
    }

    public class Productivity
    {
        public double Score { get; set; }
        public string? Developer { get; set; }
        public string? Method { get; set; }


    }

    public class MList
    {
        public Dictionary<string,List<ToolMetricValue>> metricDictionary { get; set; }   
        public Dictionary<string,List<ToolMetricValue>> metricProcDictionary { get; set; }
        public int maxCount { get; set; }
        public double score { get; set; }
    }
}
