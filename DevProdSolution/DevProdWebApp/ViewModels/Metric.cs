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
}
