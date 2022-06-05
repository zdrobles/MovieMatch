using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;

namespace MovieMatch.Models
{
    public class Rate
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public bool Thumb { get; set; }
        public int MovieId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public Rate() { }
        public Rate(MovieModel movie, bool thumb)
        {
            MovieId = movie.Id;
            MovieTitle = movie.Title;
            Thumb = thumb;
        }

        public override string ToString()
        {
            return $"{MovieId}";
        }

        public override bool Equals(object obj)
        {
            return obj is Rate rate &&
                   MovieId == rate.MovieId &&
                   ApplicationUserId == rate.ApplicationUserId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MovieId, ApplicationUserId);
        }
    }
}
