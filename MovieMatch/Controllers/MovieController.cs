using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieMatch.Data;
using MovieMatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieMatch.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext context;
        public MovieController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }
        public async Task<IActionResult> IndexAsync(int id)
        {
            MovieModel movie = await MovieProcessor.LoadMovie(id);
            bool logged = User.Identity.IsAuthenticated;
            ViewBag.logged = logged;
            if (logged)
            {
                try
                {
                    ViewBag.thumb = GetMovieRating(id).Thumb;
                }
                catch (Exception)
                {
                    Console.WriteLine("User has not rated this movie yet.");
                }
            }

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
            bool logged = User.Identity.IsAuthenticated;
            ViewBag.logged = logged;
            if (logged)
            {
                try
                {
                    ViewBag.RateList = GetUserRatings();
                    ViewBag.userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                catch (Exception)
                {
                    Console.WriteLine("User has not rated any movies");
                }
            }
            return View();
        }

        public async Task<IActionResult> SimilarAsync(int id, int page = 1)
        {
            ViewBag.page = page;
            if (page < 1 || page > 500) { return View(); }
            MovieModel movie = await MovieProcessor.LoadMovie(id);
            ViewBag.movie = movie;

            Root everything = await MovieProcessor.FindSimilar(id, page);
            ViewBag.totalPages = everything.Total_pages;
            ViewBag.movieList = everything.Results;
            bool logged = User.Identity.IsAuthenticated;
            ViewBag.logged = logged;
            if (logged)
            {
                try
                {
                    ViewBag.RateList = GetUserRatings();
                    ViewBag.userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                catch (Exception)
                {
                    Console.WriteLine("User has not rated any movies");
                }
            }
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
            bool logged = User.Identity.IsAuthenticated;
            ViewBag.logged = logged;
            if (logged)
            {
                try
                {
                    ViewBag.RateList = GetUserRatings();
                    ViewBag.userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                catch (Exception)
                {
                    Console.WriteLine("User has not rated any movies");
                }
            }
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
            bool logged = User.Identity.IsAuthenticated;
            ViewBag.logged = logged;
            if (logged)
            {
                try
                {
                    ViewBag.RateList = GetUserRatings();
                    ViewBag.userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                catch (Exception)
                {
                    Console.WriteLine("User has not rated any movies");
                }
            }
            return View("discover");
        }

        [Authorize]
        public async Task<IActionResult> RecommendationsAsync()
        {
            List<Rate> userLikes = context.Ratings
                .Where(r => r.ApplicationUserId == this.User.FindFirstValue(ClaimTypes.NameIdentifier) && r.Thumb).ToList();// only grab the user's liked movies
            List<Rate> userRatings = GetUserRatings();
            List<MovieModel> recommend = new List<MovieModel>();
            List<MovieModel> alreadyRatedMovies = new List<MovieModel>();
            foreach (Rate rate in userRatings)
            {
                MovieModel movie = await MovieProcessor.LoadMovie(rate.MovieId);
                alreadyRatedMovies.Add(movie);
            }
            foreach (Rate rating in userLikes)
            {
                //if(recommend.Count > 50) { break; }

                Root check = await MovieProcessor.FindSimilar(rating.MovieId, 1);//find total_pages and
                foreach (MovieModel movie in check.Results)//grab the first page
                {
                    if (movie.Release_date != "" && !alreadyRatedMovies.Contains(movie) && !recommend.Contains(movie))
                    {
                        recommend.Add(movie);
                    }
                }
                //for (int i = 2; i <= check.Total_pages; i++)//get all movies from every page after page 1
                //{
                //    Root add = await MovieProcessor.FindSimilar(rating.MovieId, i);
                //    foreach (MovieModel movie in add.Results)
                //    {
                //        if (!recommend.Contains(movie) && !alreadyRatedMovies.Contains(movie))
                //        {
                //            recommend.Add(movie);
                //        };
                //    }
                //}
            }

            ViewBag.movies = recommend;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> RateMovieAsync(int movieId, bool thumb)
        {
            MovieModel movie = await MovieProcessor.LoadMovie(movieId);

            AddRate(movie, thumb);

            try
            {
                ViewBag.context = GetUserRatings();
            }
            catch (Exception)
            {
                Console.WriteLine("RateMovie method failed.");
            }
            return View("Ratings");
        }


        [Authorize]
        public IActionResult Ratings()
        {
            try
            {
                ViewBag.context = GetUserRatings();
            }
            catch (Exception)
            {
                Console.WriteLine("Ratings method failed.");
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> SwitchRating(int movieId, bool thumb)
        {
            MovieModel movie = await MovieProcessor.LoadMovie(movieId);
            context.Ratings.Remove(GetMovieRating(movieId));

            AddRate(movie, thumb);
            ViewBag.context = GetUserRatings();
            return View("Ratings");
        }

        [Authorize]
        public IActionResult RemoveRating(int movieId)
        {
            context.Ratings.Remove(GetMovieRating(movieId));
            context.SaveChanges();
            ViewBag.context = GetUserRatings();
            return View("Ratings");
        }
        public void AddRate(MovieModel movie, bool thumb)
        {
            Rate rate = new Rate(movie, thumb)
            {
                ApplicationUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            context.Ratings.Add(rate);
            context.SaveChanges();
        }
        public List<Rate> GetUserRatings()
        {
            return context.Ratings.Where(r => r.ApplicationUserId == this.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
        }

        public Rate GetMovieRating(int movieId)
        {
            return context.Ratings.Single(r => r.ApplicationUserId == this.User.FindFirstValue(ClaimTypes.NameIdentifier) && r.MovieId == movieId);
        }
    }
}
