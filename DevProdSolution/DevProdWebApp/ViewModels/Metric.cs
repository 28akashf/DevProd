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
}
