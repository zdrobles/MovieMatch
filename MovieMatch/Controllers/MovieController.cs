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

        public async Task<IActionResult> PopularAsync(int page = 1)
        {
            Root everything = await MovieProcessor.GetPopular(page);
            ViewBag.totalPages = everything.Total_pages;
            ViewBag.movieList = everything.Results;
            ViewBag.page = page;
            return View();
        }

        public async Task<IActionResult> SimilarAsync(int id, int page = 1)
        {
            MovieModel movie = await MovieProcessor.LoadMovie(id);
            ViewBag.movie = movie;

            List<string> keywords = await MovieProcessor.GetKeywords(id);

            ViewBag.keywords = string.Join(", ", keywords);

            Root everything = await MovieProcessor.FindSimilar(id, page);
            ViewBag.totalPages = everything.Total_pages;
            ViewBag.movieList = everything.Results;
            ViewBag.page = page;
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchMoviesAsync(string query, int page = 1)
        {
            Root everything = await MovieProcessor.Search(query, page);
            ViewBag.totalPages = everything.Total_pages;
            ViewBag.movieList = everything.Results;
            ViewBag.page = page;
            ViewBag.query = query;
            return View("search");
        }
    }
}
