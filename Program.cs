using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using AspCoreFirstApp.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<AuditSaveChangesInterceptor>();
// Register the DbContext
builder.Services.AddDbContext<ApplicationdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationdbContext>();

    // Seed Genres
    if (!db.Genres.Any())
    {
        var genrePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Genre.json");
        if (File.Exists(genrePath))
        {
            var genreJson = File.ReadAllText(genrePath);
            var genres = System.Text.Json.JsonSerializer.Deserialize<List<Genre>>(genreJson);
            if (genres != null)
            {
                db.Genres.AddRange(genres);
                db.SaveChanges();
            }
        }
    }

    // Seed Movies
    if (!db.Movies.Any())
    {
        var moviePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Movie.json");
        if (File.Exists(moviePath))
        {
            var movieJson = File.ReadAllText(moviePath);
            var movies = System.Text.Json.JsonSerializer.Deserialize<List<Movie>>(movieJson);
            if (movies != null)
            {
                db.Movies.AddRange(movies);
                db.SaveChanges();
            }
        }
    }
}


// Map the custom route (this is fine here, as it's on the app)
app.MapControllerRoute(
    name: "MovieByRelease",
    pattern: "Movie/released/{year}/{month}",
    defaults: new { controller = "Movie", action = "ByRelease" }
);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();