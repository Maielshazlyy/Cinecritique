using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCritique.domain.Models
{
    public class AdminFollower
    {
        public string AdminId { get; set; }
        public Admin Admin { get; set; }

        public int FollowerId { get; set; }
        public Follower Follower { get; set; }
    }
}
