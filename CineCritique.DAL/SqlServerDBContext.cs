using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineCritique.domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // ✅ الصحيح لـ EF Core

using Microsoft.EntityFrameworkCore;

namespace CineCritique.DAL
{
    public class SqlServerDBContext : IdentityDbContext<User>
    {
        public SqlServerDBContext(DbContextOptions<SqlServerDBContext> options) : base(options)
        {

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }  // إضافة DbSet لـ Admin
        public DbSet<Follower> Followers { get; set; }
        public DbSet<CriticArticle> CriticalArticles { get; set; }
        public DbSet<ChatBot> ChatMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // إضافة المفتاح الرئيسي لـ IdentityUserLogin

            // تحديد العلاقة بين Admin و Follower عبر جدول الربط AdminFollower
            modelBuilder.Entity<AdminFollower>()
                .HasKey(af => new { af.AdminId, af.FollowerId });  // المفتاح المركب (AdminId + FollowerId)

            modelBuilder.Entity<AdminFollower>()
                .HasOne(af => af.Admin)
                .WithMany(a => a.AdminFollowers)  // Admin له العديد من AdminFollowers
                .HasForeignKey(af => af.AdminId)
                .OnDelete(DeleteBehavior.Restrict);  // استخدام Restrict بدلاً من Cascade لتجنب الحلقات

            modelBuilder.Entity<AdminFollower>()
                .HasOne(af => af.Follower)
                .WithMany(f => f.AdminFollowers)  // Follower له العديد من AdminFollowers
                .HasForeignKey(af => af.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);  // استخدام Restrict بدلاً من Cascade لتجنب الحلقات

            // تحديد العلاقة بين Follower و User
            modelBuilder.Entity<Follower>()
                .HasOne(f => f.FollowerUser)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowerUserId)
                .OnDelete(DeleteBehavior.Restrict);  // استخدام Restrict بدلاً من Cascade لتجنب الحلقات

            // تحديد العلاقة بين Follower و User الذي يتابعه
            modelBuilder.Entity<Follower>()
    .HasOne(f => f.FollowingUser)
    .WithMany() // لا تمرر خاصية غير موجودة
    .HasForeignKey(f => f.FollowingUserId)
    .OnDelete(DeleteBehavior.Restrict); //استخدام Restrict بدلاً من Cascade لتجنب الحلقات
        }





    }
}
