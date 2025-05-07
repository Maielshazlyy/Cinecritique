using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCritique.domain.Models
{
  public class Reviews
    {
        public int Id { get; set; }  
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }  
        public string Content { get; set; } 
        public DateTime DateCreated { get; set; } 
    }
}
