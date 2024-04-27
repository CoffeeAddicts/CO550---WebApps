using FitToFight.Models;
using FitToFight.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitToFight.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly FitToFightContext _context;
        private readonly UserManager<User> _userManager;

        public User appUser { get; set; }

        public IndexModel(FitToFightContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public AdminViewModal ViewModel { get; set; } = default!;

        public void OnGet()
        {
            //var task = _userManager.GetUserAsync(User);
            //task.Wait();
            //appUser = task.Result;

            //_userManager.AddToRoleAsync(appUser, "admin").Wait();

            ViewModel = new AdminViewModal();
            ViewModel.HomePage = _context.HomePageData.ToList();
        }
    }
}
