using System.ComponentModel.DataAnnotations;

namespace AspCoreFirstApp.Models;

public class Genre
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Le nom du genre est obligatoire.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères.")]
    public string? Name { get; set; }
    public List<Movie> Movies { get; set; } = new List<Movie>();

}
