using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Pw { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime Day { get; set; }
        [Required]
        public string Hp { get; set; }
    }
}
