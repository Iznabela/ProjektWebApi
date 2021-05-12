﻿using Microsoft.AspNet.Identity;
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
            Seed(modelBuilder);
        }

        private static void Seed(ModelBuilder modelBuilder)
        {
            var messageOne = new
            {
                Id = 1,
                Title = "Bellas place",
                Body = "Här bor Bella! Stay away",
                Author = "Unknown Author"
            };

            var messageTwo = new
            {
                Id = 2,
                Title = "Andra långgatan",
                Body = "Bästa stället att dricka öl!",
                Author = "Unknown Author"
            };

            var geoMessageOne = new
            {
                Id = 1,
                Longitude = 57.873718295961204,
                Latitude = 11.970617969653047,
                MessageId = 1
            };

            var geoMessageTwo = new
            {
                Id = 2,
                Longitude = 57.699100041459346,
                Latitude = 11.946499084988522,
                MessageId = 2
            };

            modelBuilder.Entity<Message>().HasData(messageOne);

            modelBuilder.Entity<Message>().HasData(messageTwo);

            modelBuilder.Entity<GeoMessage>().HasData(geoMessageOne);

            modelBuilder.Entity<GeoMessage>().HasData(geoMessageTwo);

            string password = "test123";
            PasswordHasher passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.HashPassword(password);

            MyUser user = new MyUser()
            {
                UserName = "testuser",
                FirstName = "Test",
                LastName = "Testsson",
                PasswordHash = hashedPassword
            };

            modelBuilder.Entity<MyUser>().HasData(user);
        }
    }
}
