using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCritique.domain.Models
{
    public class Follower
    {
        public int Id { get; set; }
        //public int UserId { get; set; }
        //public User User { get; set; }
     //   public int FollowerId { get; set; }
        public User FollowerUser { get; set; }
        public string FollowerUserId { get; set; }
        public string FollowingUserId { get; set; }
      // public int FollowingId { get; set; }
        public User FollowingUser { get; set; }
       

        public List<AdminFollower> AdminFollowers { get; set; }
        public List<AdminFollower> AdminFollowing { get; set; }
    }
}
