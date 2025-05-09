using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCritique.DAL.DTOS
{
  public  class AddMovieToPlaylistDto
    {
        public int PlaylistId { get; set; }
        public int MovieId { get; set; }
    }
}
