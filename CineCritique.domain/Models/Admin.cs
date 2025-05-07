using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineCritique.domain.Models
{
    public class Admin : User
    {
        public bool HasFullAccess { get; set; }  // صلاحيات كاملة (مثل إدارة الأفلام، المراجعات، المستخدمين)
        public List<Movie> ManagedMovies { get; set; }  // قائمة الأفلام التي يديرها الـ Admin
        public List<Reviews> ManagedReviews { get; set; }  // قائمة المراجعات التي يديرها الـ Admin

        public List<AdminFollower> AdminFollowers { get; set; }
        public List<AdminFollower> AdminFollowing { get; set; }
    }
}