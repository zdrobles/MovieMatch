using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieMatch.Models
{
    public class MovieProcessor
    {
        private static Random random = new Random();
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

        //public static async Task<MovieModel> LoadMovie()
        //{
        //    int maxNum = 0;
        //    using (HttpResponseMessage resp = await APIHelper.ApiClient.GetAsync("https://api.themoviedb.org/3/movie/latest?api_key=23772da152380f0f559f6ef4456ca9c1"))
        //    {
        //        if (resp.IsSuccessStatusCode)
        //        {
        //            MovieModel mov = await resp.Content.ReadAsAsync<MovieModel>();
        //            maxNum = mov.Id;
        //        }
        //        else
        //        {
        //            throw new Exception(resp.ReasonPhrase);
        //        }
        //    }

        //    string url = $"https://api.themoviedb.org/3/movie/{random.Next(9, maxNum)}?api_key=23772da152380f0f559f6ef4456ca9c1";
        //    HttpResponseMessage response;
        //    MovieModel movie = new MovieModel();
        //    using (response = await APIHelper.ApiClient.GetAsync(url))
        //    {
        //        while (!response.IsSuccessStatusCode)
        //        {
        //            using (response = await APIHelper.ApiClient.GetAsync($"https://api.themoviedb.org/3/movie/{random.Next(9, maxNum)}?api_key=23772da152380f0f559f6ef4456ca9c1"))
        //            {
        //                if (!response.IsSuccessStatusCode) { continue; }
        //                movie = await response.Content.ReadAsAsync<MovieModel>();
        //            }
        //        }
        //        return movie;
        //    }
        //}

        public static async Task<List<MovieModel>> GetPopular(int num)
        {

            string url = $"https://api.themoviedb.org/3/movie/popular?api_key=23772da152380f0f559f6ef4456ca9c1&language=en-US&page={num}";
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string myJsonResponse = await response.Content.ReadAsStringAsync();
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

                    List<MovieModel> movies = myDeserializedClass.Results;
                    return movies;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public static async Task<List<MovieModel>> FindSimilar(int id, int num)
        {
            string url = $"https://api.themoviedb.org/3/movie/{id}/similar?api_key=23772da152380f0f559f6ef4456ca9c1&language=en-US&page={num}";
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string myJsonResponse = await response.Content.ReadAsStringAsync();
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

                    List<MovieModel> movies = myDeserializedClass.Results;
                    return movies;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<List<string>> GetKeywords(int id)
        {
            string url = $"https://api.themoviedb.org/3/movie/{id}/keywords?api_key=23772da152380f0f559f6ef4456ca9c1";
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<string> keywords = new List<string>();
                    string myJsonResponse = await response.Content.ReadAsStringAsync();
                    KeywordRoot myDeserializedClass = JsonConvert.DeserializeObject<KeywordRoot>(myJsonResponse);

                    foreach (var key in myDeserializedClass.Keywords)
                    {
                        keywords.Add(key.Name);
                    }
                    return keywords;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
