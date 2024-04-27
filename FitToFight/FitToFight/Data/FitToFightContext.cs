using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FitToFight.Models;

public class FitToFightContext : DbContext
{
    public FitToFightContext(DbContextOptions<FitToFightContext> options)
        : base(options)
    {
    }

    public DbSet<HomePageData> HomePageData { get; set; } = default!;
    public DbSet<Class> Classes { get; set; }
    public DbSet<ActiveClass> ActiveClasses { get; set; }
    public DbSet<AppSetting> AppSettings { get; set; }
}
