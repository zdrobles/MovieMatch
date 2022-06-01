using System.Collections.Generic;

namespace MovieMatch.Models
{
    public class MovieModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Release_date { get; set; }
        public string Poster_path { get; set; }
        public string Overview { get; set; }
        public string Imdb_id { get; set; }

    }

    public class Root
    {
        //public int Page { get; set; }
        public List<MovieModel> Results { get; set; }
        //public int Total_pages { get; set; }
        //public int Total_results { get; set; }
    }

    public class KeywordRoot
    {
        public List<Keyword> Keywords { get; set; }
    }
    public class Keyword
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}