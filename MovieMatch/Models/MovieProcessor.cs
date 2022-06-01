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

        public static async Task<Root> GetPopular(int page)
        {

            string url = $"https://api.themoviedb.org/3/movie/popular?api_key=23772da152380f0f559f6ef4456ca9c1&language=en-US&page={page}";
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string myJsonResponse = await response.Content.ReadAsStringAsync();
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

                    //List<MovieModel> movies = myDeserializedClass.Results;
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

                    //List<MovieModel> movies = myDeserializedClass.Results;
                    return myDeserializedClass;
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

        public static async Task<Root> Search(string query, int page)
        {
            string url = $"https://api.themoviedb.org/3/search/movie?api_key=23772da152380f0f559f6ef4456ca9c1&language=en-US&query={query}&page={page}&include_adult=false";
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    string myJsonResponse = await response.Content.ReadAsStringAsync();
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

                    //List<MovieModel> movies = myDeserializedClass.Results;
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
