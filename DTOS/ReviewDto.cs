using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCritique.DAL.DTOS
{
  
        public class ReviewDto
        {
            public int MovieId { get; set; }
            public int UserId { get; set; }
            public string Content { get; set; }
            public DateTime DateCreated { get; set; }
        }
}



