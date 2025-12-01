using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Models
{
    public class ApplicationdbContext : DbContext
    {
        public ApplicationdbContext(DbContextOptions<ApplicationdbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

    
            var genrePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Genre.json");
            if (File.Exists(genrePath))
            {
                var genreJson = File.ReadAllText(genrePath);
                var genres = System.Text.Json.JsonSerializer.Deserialize<List<Genre>>(genreJson);

                if (genres != null)
                    modelBuilder.Entity<Genre>().HasData(genres);
            }

            var moviePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Movie.json");
            if (File.Exists(moviePath))
            {
                var movieJson = File.ReadAllText(moviePath);
                var movies = System.Text.Json.JsonSerializer.Deserialize<List<Movie>>(movieJson);

                if (movies != null)
                    modelBuilder.Entity<Movie>().HasData(movies);
            }
        }

    }
}
