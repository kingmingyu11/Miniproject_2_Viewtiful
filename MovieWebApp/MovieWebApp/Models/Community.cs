using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Models
{
    public class Community
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Writer { get; set; }
    }
}
