﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitToFight.Models;

namespace FitToFight.Pages
{
    public class EditModel : PageModel
    {
        private readonly FitToFightContext _context;

        public EditModel(FitToFightContext context)
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

            var homepagedata =  await _context.HomePageData.FirstOrDefaultAsync(m => m.Key == id);
            if (homepagedata == null)
            {
                return NotFound();
            }
            HomePageData = homepagedata;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(HomePageData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomePageDataExists(HomePageData.Key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool HomePageDataExists(string id)
        {
          return (_context.HomePageData?.Any(e => e.Key == id)).GetValueOrDefault();
        }
    }
}
