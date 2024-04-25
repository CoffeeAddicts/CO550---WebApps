using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FitToFight.Models;

    public class FitToFightContext : DbContext
    {
        public FitToFightContext (DbContextOptions<FitToFightContext> options)
            : base(options)
        {
        }

        public DbSet<FitToFight.Models.HomePageData> HomePageData { get; set; } = default!;
    }
