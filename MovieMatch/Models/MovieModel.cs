using System;
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
        public string Tagline { get; set; }

        public double Vote_Average { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MovieModel model &&
                   Id == model.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return $"{Title}";
        }


    }

    public class Root
    {
        //public int Page { get; set; }
        public List<MovieModel> Results { get; set; }
        public int Total_pages { get; set; }
        //public int Total_results { get; set; }
    }

}