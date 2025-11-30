namespace AspCoreFirstApp.ViewModels
{
    using AspCoreFirstApp.Models;
    using Microsoft.AspNetCore.Http;
    public class MovieVM
    {
        public Movie movie { get; set; }
        public IFormFile? photo { get; set; }
    }
}