using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCritique.DAL.DTOS
{
  public  class CreatePlaylistDto
    {
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
