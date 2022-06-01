using Microsoft.AspNetCore.Mvc;
using MovieMatch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieMatch.Controllers
{
    public class MovieController : Controller
    {
        [HttpGet("/movie/{id}")]
        public async Task<IActionResult> IndexAsync(int id)
        {
            MovieModel movie = await MovieProcessor.LoadMovie(id);
            ViewBag.movie = movie;
            return View();
        }

        [HttpGet("/popular/{num?}")]
        public async Task<IActionResult> PopularAsync(int num = 1)
        {
            List<MovieModel> movieList = await MovieProcessor.GetPopular(num);
            ViewBag.movieList = movieList;
            ViewBag.num = num;
            return View();
        }
        [HttpGet("/movie/{id}/similar/{num?}")]
        public async Task<IActionResult> SimilarAsync(int id, int num = 1)
        {
            MovieModel movie = await MovieProcessor.LoadMovie(id);
            ViewBag.movie = movie;

            List<string> keywords = await MovieProcessor.GetKeywords(id);

            ViewBag.keywords = string.Join(", ", keywords);

            List<MovieModel> movieList = await MovieProcessor.FindSimilar(id, num);
            ViewBag.movieList = movieList;
            ViewBag.num = num;
            return View();
        }
    }
}
