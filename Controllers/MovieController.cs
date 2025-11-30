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
    public MovieController(ApplicationdbContext db)
    {
        _db = db;
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
    public IActionResult Create(Movie movie)
    {
        if (ModelState.IsValid)
        {
            _db.Movies.Add(movie);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Genres = _db.Genres.ToList();
        return View(movie);
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
        var movie = _db.Movies.FirstOrDefault(m => m.Id == id);
        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var movie = _db.Movies.Find(id);
        _db.Movies.Remove(movie);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
