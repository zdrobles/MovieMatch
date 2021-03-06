using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieMatch.Models
{
    public class MovieProcessor
    {
        public static async Task<MovieModel> LoadMovie(int num)
        {
            string url = $"https://api.themoviedb.org/3/movie/{num}?api_key=23772da152380f0f559f6ef4456ca9c1";
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    MovieModel movie = await response.Content.ReadAsAsync<MovieModel>();
                    return movie;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<Root> GetMovies(int page, string selection)
        {
            string url = $"https://api.themoviedb.org/3/movie/{selection}?api_key=23772da152380f0f559f6ef4456ca9c1&language=en-US&region=US&page={page}";
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string myJsonResponse = await response.Content.ReadAsStringAsync();
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

                    return myDeserializedClass;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public static async Task<Root> FindSimilar(int id, int page)
        {
            string url = $"https://api.themoviedb.org/3/movie/{id}/recommendations?api_key=23772da152380f0f559f6ef4456ca9c1&language=en-US&page={page}";
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string myJsonResponse = await response.Content.ReadAsStringAsync();
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

                    return myDeserializedClass;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<Root> Search(string query, int page)
        {
            string url = $"https://api.themoviedb.org/3/search/movie?api_key=23772da152380f0f559f6ef4456ca9c1&language=en-US&region=US&query={query}&page={page}&include_adult=false";
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string myJsonResponse = await response.Content.ReadAsStringAsync();
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

                    return myDeserializedClass;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public static async Task<Root> Discover(int page, string year, string genres)
        {
            string url = $"https://api.themoviedb.org/3/discover/movie?api_key=23772da152380f0f559f6ef4456ca9c1&include_adult=false&include_video=false&page={page}";

            if(year != null)
            {
                url += $"&primary_release_year={year}";
            }
            if(genres != null)
            {
                url += $"with_genres={genres}";
            }
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string myJsonResponse = await response.Content.ReadAsStringAsync();
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

                    return myDeserializedClass;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
