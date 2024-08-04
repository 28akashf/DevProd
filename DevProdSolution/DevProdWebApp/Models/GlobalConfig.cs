using System.ComponentModel.DataAnnotations;

namespace DevProdWebApp.Models
{
    public class GlobalConfig
    {
        [Key]
        public int Id { get; set; }
        public string? ConfigKey { get; set; }
        public string? ConfigValue { get; set; }
    }
}
