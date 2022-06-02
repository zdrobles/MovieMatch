using Microsoft.AspNetCore.Mvc;
using MovieMatch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieMatch.Controllers
{
    public class MovieController : Controller
    {
        public async Task<IActionResult> IndexAsync(int id)
        {
            MovieModel movie = await MovieProcessor.LoadMovie(id);
            ViewBag.movie = movie;
            return View();
        }

        public async Task<IActionResult> FetchAsync(string selection, int page = 1)
        {
            ViewBag.page = page;
            ViewBag.selection = selection;
            if (page < 1 || page > 500) { return View(); }
            Root everything = await MovieProcessor.GetMovies(page, selection.ToLower());
            ViewBag.totalPages = everything.Total_pages;
            ViewBag.movieList = everything.Results;

            return View();
        }

        public async Task<IActionResult> SimilarAsync(int id, int page = 1)
        {
            ViewBag.page = page;
            if (page < 1 || page > 500) { return View(); }
            MovieModel movie = await MovieProcessor.LoadMovie(id);
            ViewBag.movie = movie;
            
            //List<string> keywords = await MovieProcessor.GetKeywords(id);

            //ViewBag.keywords = string.Join(", ", keywords);

            Root everything = await MovieProcessor.FindSimilar(id, page);
            ViewBag.totalPages = everything.Total_pages;
            ViewBag.movieList = everything.Results;

            return View();
        }
                
        public async Task<IActionResult> SearchAsync(string query, int page = 1)
        {
            if (page < 1) { page = 1; }
            Root everything = await MovieProcessor.Search(query, page);
            ViewBag.totalPages = everything.Total_pages;
            ViewBag.movieList = everything.Results;
            ViewBag.page = page;
            ViewBag.query = query;
            return View();
        }

        public IActionResult Discover()
        {
            return View();
        }
        public async Task<IActionResult> DiscoverMoviesAsync(string year, string genres, int page = 1)
        {
            if (page < 1) { page = 1; }
            Root everything = await MovieProcessor.Discover(page, year, genres);
            ViewBag.totalPages = everything.Total_pages;
            ViewBag.movieList = everything.Results;
            ViewBag.page = page;
            ViewBag.year = year;
            return View("discover");
        }
    }
}
