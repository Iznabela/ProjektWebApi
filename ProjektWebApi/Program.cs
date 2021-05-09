using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjektWebApi.Data;
using ProjektWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
                Seeding(host);
                host.Run();
        }

        static async void Seeding(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var Dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MyUser>>();

                MyUser testuser1 = new MyUser()
                {
                    FirstName = "test1",
                    LastName = "user1",
                    UserName = "test1",
                    Email = "testuser1@gmail.com",
                    
                };
                await userManager.CreateAsync(testuser1, "test123");
                await Dbcontext.SaveChangesAsync();
            }

        }




        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
