using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;

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


        }

        public void SeedFromJson()
        {

            var genrePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Genre.json");
            if (File.Exists(genrePath) && !Genres.Any())
            {
                var genreJson = File.ReadAllText(genrePath);
                var genres = JsonSerializer.Deserialize<List<Genre>>(genreJson);
                if (genres != null)
                {
                    Genres.AddRange(genres);
                    SaveChanges();
                }
            }


            var moviePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Movie.json");
            if (File.Exists(moviePath) && !Movies.Any())
            {
                var movieJson = File.ReadAllText(moviePath);
                var movies = JsonSerializer.Deserialize<List<Movie>>(movieJson);
                if (movies != null)
                {
                    Movies.AddRange(movies);
                    SaveChanges();
                }
            }
        }
    }
}
