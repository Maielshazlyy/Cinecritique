using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;

namespace CineCritique.domain.Models
{
  public  class User: IdentityUser
    {
    // public int Id { get; set; }
     //   public string Username { get; set; }  
      // public string Email { get; set; }  
//public string PasswordHash { get; set; }  
        public string Role { get; set; }  //top user or regular
        public List<Reviews> Reviews { get; set; }
        public List<Follower> Followers { get; set; }  
        public List<Follower> Following { get; set; }
        public bool IsBanned { get; set; }
        public bool IsActive { get; set; }
      //  public string Username { get; set; }
    }
}
