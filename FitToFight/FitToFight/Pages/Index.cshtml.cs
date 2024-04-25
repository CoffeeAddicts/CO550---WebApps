using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FitToFight.Models;

namespace FitToFight.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FitToFightContext _context;

        public IndexModel(FitToFightContext context)
        {
            _context = context;
        }

        public IList<HomePageData> HomePageData { get;set; } = default!;

        public async Task<ActionResult> OnGetAsync()
        {
            HomePageViewModel model = new HomePageViewModel();
            if (_context.HomePageData != null)
            {
                var test = await _context.HomePageData.ToListAsync();
                HomePageData = test;

                //HomePageData = await _context.HomePageData.ToListAsync();
            }
            return Page();

        }
    }
}
