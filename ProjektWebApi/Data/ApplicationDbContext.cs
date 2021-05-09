using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektWebApi.Models;

namespace ProjektWebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GeoMessage> GeoMessages { get; set; }
        public DbSet<MyUser> MyUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GeoMessage>().ToTable("GeoMessage");
            SeedUsers(modelBuilder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            MyUser user = new MyUser()
            {
                UserName = "TestUser",
                FirstName = "Test",
                LastName = "Testsson"
            };

            PasswordHasher<MyUser> passwordHasher = new PasswordHasher<MyUser>();
            passwordHasher.HashPassword(user, "Test1234%");

            builder.Entity<MyUser>().HasData(user);
        }
    }
}
