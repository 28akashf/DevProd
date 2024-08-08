using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevProdWebApp.Models
{
    public class Developer
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        [ForeignKey("Setting")]
        public int? SettingId { get; set; }
        public Setting? Setting { get; set; }
    }
}
