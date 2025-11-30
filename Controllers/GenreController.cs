namespace AspCoreFirstApp.Controllers;
 
 using Microsoft.AspNetCore.Mvc;
 using Microsoft.EntityFrameworkCore;
 using AspCoreFirstApp.Models;
 using AspCoreFirstApp.ViewModels;
 using AspCoreFirstApp.Helpers;

public class GenreController : Controller
{
    private readonly ApplicationdbContext _db;

    public GenreController(ApplicationdbContext db)
    {
        _db = db;
    }

    public IActionResult Index(int page = 1, string sortBy = "Id", string sortOrder = "asc", int pageSize = 10)
    {
        // Récupérer tous les genres
        var query = _db.Genres.AsQueryable();

        // Appliquer le tri
        query = sortBy.ToLower() switch
        {
            "name" => sortOrder == "desc" ? query.OrderByDescending(g => g.Name) : query.OrderBy(g => g.Name),
            "id" => sortOrder == "desc" ? query.OrderByDescending(g => g.Id) : query.OrderBy(g => g.Id),
            _ => query.OrderBy(g => g.Id)
        };

        // Total avant pagination
        int totalCount = query.Count();

        // Appliquer la pagination
        var paginatedGenres = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        // Créer le helper de pagination
        var pagination = new PaginationHelper(totalCount, page, pageSize);

        // Créer et retourner le ViewModel paginé
        var viewModel = new PaginatedListViewModel<Genre>(paginatedGenres, pagination, sortBy, sortOrder);
        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Genre genre)
    {
        if (ModelState.IsValid)
        {
            _db.Genres.Add(genre);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(genre);
    }

    public IActionResult Edit(Guid id)
    {
        var genre = _db.Genres.FirstOrDefault(g => g.Id == id);
        if (genre == null)
            return NotFound();
        return View(genre);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, Genre genre)
    {
        if (id != genre.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _db.Genres.Update(genre);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Erreur lors de la mise à jour du genre.");
            }
        }
        return View(genre);
    }

    public IActionResult Delete(Guid id)
    {
        var genre = _db.Genres.Include(g => g.Movies).FirstOrDefault(g => g.Id == id);
        if (genre == null)
            return NotFound();
        return View(genre);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        var genre = _db.Genres.Find(id);
        if (genre != null)
        {
            _db.Genres.Remove(genre);
            _db.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}
