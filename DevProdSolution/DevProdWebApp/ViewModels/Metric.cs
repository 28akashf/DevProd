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
        public List<int> m1List { get; set; }
        public List<double> m2List { get; set; }
        public List<int> m3List { get; set; }
        public int maxCount { get; set; }
    }
}
