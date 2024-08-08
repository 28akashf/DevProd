using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevProdWebApp.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }         
        public string? Name { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Setting")]
        public int? SettingId { get; set; }
        public Setting? Setting { get; set; }

        // public List<Metric>? ProjectMetrics { get; set; }

        //  public List<Developer>? Developers { get; set;}
    }
}
