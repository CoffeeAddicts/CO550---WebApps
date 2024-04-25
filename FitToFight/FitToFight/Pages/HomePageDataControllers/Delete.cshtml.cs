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
    public class DeleteModel : PageModel
    {
        private readonly FitToFightContext _context;

        public DeleteModel(FitToFightContext context)
        {
            _context = context;
        }

        [BindProperty]
      public HomePageData HomePageData { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.HomePageData == null)
            {
                return NotFound();
            }

            var homepagedata = await _context.HomePageData.FirstOrDefaultAsync(m => m.Key == id);

            if (homepagedata == null)
            {
                return NotFound();
            }
            else 
            {
                HomePageData = homepagedata;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.HomePageData == null)
            {
                return NotFound();
            }
            var homepagedata = await _context.HomePageData.FindAsync(id);

            if (homepagedata != null)
            {
                HomePageData = homepagedata;
                _context.HomePageData.Remove(HomePageData);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
