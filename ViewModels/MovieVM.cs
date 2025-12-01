namespace AspCoreFirstApp.ViewModels
{
    using AspCoreFirstApp.Models;
    using Microsoft.AspNetCore.Http;
    public class MovieVM
    {
        public required Movie movie { get; set; }
        public IFormFile? photo { get; set; }

        public MovieVM()
        {
            movie = new Movie();
        }
    }
}