using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AspCoreFirstApp.Controllers;

using AspCoreFirstApp.Models;
using AspCoreFirstApp.ViewModels;
using AspCoreFirstApp.Helpers;

public class MovieController : Controller
{
    private readonly ApplicationdbContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public MovieController(ApplicationdbContext db, IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index(int page = 1, string sortBy = "Id", string sortOrder = "asc", int pageSize = 10)
    {
        // Récupérer tous les films avec leurs genres
        var query = _db.Movies.Include(m => m.Genre).AsQueryable();

        // Appliquer le tri
        query = sortBy.ToLower() switch
        {
            "name" => sortOrder == "desc" ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name),
            "id" => sortOrder == "desc" ? query.OrderByDescending(m => m.Id) : query.OrderBy(m => m.Id),
            _ => query.OrderBy(m => m.Id)
        };

        // Total avant pagination
        int totalCount = query.Count();

        // Appliquer la pagination
        var paginatedMovies = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        // Créer le helper de pagination
        var pagination = new PaginationHelper(totalCount, page, pageSize);

        // Créer et retourner le ViewModel paginé
        var viewModel = new PaginatedListViewModel<Movie>(paginatedMovies, pagination, sortBy, sortOrder);
        return View(viewModel);
    }

    public IActionResult ByRelease(int year, int month)
    {
        return Content($"Films sortis en {month}/{year}");
    }

    public IActionResult Create()
    {
        ViewBag.Genres = _db.Genres.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(MovieVM model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return View(model);
        }

        var photo = model.photo;
        if (photo == null || photo.Length == 0)
        {
            ModelState.AddModelError("", "Fichier non envoyé.");
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return View(model);
        }

        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
        var filePath = Path.Combine(uploads, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            photo.CopyTo(stream);
        }

        var movie = new Movie
        {
            Id = model.movie.Id,
            Name = model.movie.Name,
            DateAjoutMovie = model.movie.DateAjoutMovie ?? DateTime.UtcNow,
            ImageFile = fileName
        };

        _db.Movies.Add(movie);
        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int id)
    {
        var customer = new Customer { Id = id, Name = "Client Test" };

        var movies = new List<Movie>
    {
        new Movie { Id = 1, Name = "Movie A" },
        new Movie { Id = 2, Name = "Movie B" }
    };

        var vm = new MovieCustomerViewModel
        {
            Customer = customer,
            Movies = movies
        };

        return View(vm);
    }

    public IActionResult Edit(int id)
    {
        var movie = _db.Movies.FirstOrDefault(m => m.Id == id);
        if (movie == null)
            return NotFound();
        ViewBag.Genres = _db.Genres.ToList();
        return View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Movie movie)
    {
        if (id != movie.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _db.Movies.Update(movie);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Erreur lors de la mise à jour du film.");
            }
        }
        ViewBag.Genres = _db.Genres.ToList();
        return View(movie);
    }

    public IActionResult Delete(int id)
    {
        var movie = _db.Movies.Include(m => m.Genre).FirstOrDefault(m => m.Id == id);
        if (movie == null)
            return NotFound();
        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var movie = _db.Movies.Find(id);
        if (movie != null)
        {
            _db.Movies.Remove(movie);
            _db.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}
