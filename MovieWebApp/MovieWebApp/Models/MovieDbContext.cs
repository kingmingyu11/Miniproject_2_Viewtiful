using Microsoft.EntityFrameworkCore;
using MovieWebApp.Models;

namespace MovieWebApp.Models
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<Community> Community { get; set; }
        public DbSet<UserFavoriteMovie> UserFavoriteMovie { get; set; }
    }
}
