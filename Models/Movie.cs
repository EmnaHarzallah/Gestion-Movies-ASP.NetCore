namespace AspCoreFirstApp.Models;
using System;
using System.Collections.Generic;

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Guid? GenreId { get; set; }
    public Genre? Genre { get; set; }
}
