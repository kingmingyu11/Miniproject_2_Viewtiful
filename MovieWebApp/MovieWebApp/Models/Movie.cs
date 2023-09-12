using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieWebApp.Models
{
    public class Movie
    {
        [Key]
        public string Title { get; set; }
        [Required]
        public string Contents { get; set; }
        
        [Column(TypeName = "varbinary(max)")]
        public byte[]? Video { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Age { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int Views { get; set; }
        
        [Column(TypeName = "varbinary(max)")]
        public byte[]? Picture { get; set; }
    }
}
