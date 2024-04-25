using System;
using System.Linq;
using FitToFight.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FitToFightContext context)
        {
            //context.Database.EnsureCreated();

            // Look for any students.
            if (context.HomePageData.Any())
            {
                return;   // DB has been seeded
            }

            var HomePageData = new HomePageData[]
            {
                new HomePageData { Key = "FirstContainer",   Data= "This is the first Container " },
            };

            context.HomePageData.AddRange(HomePageData);
            context.SaveChanges();
        }
    }
}