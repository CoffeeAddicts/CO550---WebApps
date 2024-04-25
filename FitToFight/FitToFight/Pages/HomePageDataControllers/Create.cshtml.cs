using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FitToFight.Models;

namespace FitToFight.Pages.HomePageDataControllers
{
    public class CreateModel : PageModel
    {
        private readonly FitToFightContext _context;

        public CreateModel(FitToFightContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public HomePageData HomePageData { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.HomePageData == null || HomePageData == null)
            {
                return Page();
            }

            _context.HomePageData.Add(HomePageData);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
