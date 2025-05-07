using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCritique.DAL.DTOS
{
   public class MovieDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int GenreId { get; set; }

        public int Year { get; set; }

        public string Description { get; set; }

        public string PosterUrl { get; set; }

        public double Rating { get; set; }
        public int Id { get; set; }
    }
}
