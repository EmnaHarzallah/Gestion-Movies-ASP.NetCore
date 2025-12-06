using Microsoft.EntityFrameworkCore;
using AspCoreFirstApp.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));

// Add services
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationdbContext>();


    db.Database.Migrate();

    static List<T>? LoadJsonData<T>(string path)
    {
        if (!File.Exists(path)) return null;

        var json = File.ReadAllText(path);
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return System.Text.Json.JsonSerializer.Deserialize<List<T>>(json, options);
    }

    // Seed Genres first
    if (!db.Genres.Any())
    {
        var genres = LoadJsonData<Genre>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Genre.json"));
        if (genres != null && genres.Any())
        {
            db.Genres.AddRange(genres);
            db.SaveChanges();
        }
    }

    // Seed Movies
    if (!db.Movies.Any())
    {
        var movies = LoadJsonData<Movie>(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Movie.json"));

        // Ensure GUIDs are properly parsed
        if (movies != null && movies.Any())
        {
            foreach (var movie in movies)
            {
                if (movie.GenreId == Guid.Empty)
                {
                    Console.WriteLine($"Movie '{movie.Name}' has an invalid GenreId.");
                }
            }

            db.Movies.AddRange(movies);
            db.SaveChanges();
        }
    }
}



app.MapControllerRoute(
    name: "MovieByRelease",
    pattern: "Movie/released/{year}/{month}",
    defaults: new { controller = "Movie", action = "ByRelease" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.Run();
