using FitToFight.Models;
using FitToFight.Models.ViewModels;
using FitToFight.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FitToFight.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly FitToFightContext _context;
        private readonly ApplicationDbContext _appContext;
        private readonly UserManager<User> _userManager;

        public User appUser { get; set; }

        public IndexModel(FitToFightContext context, UserManager<User> userManager, ApplicationDbContext appContext)
        {
            _appContext = appContext;
            _context = context;
            _userManager = userManager;
        }
        public AdminViewModal ViewModel { get; set; } = default!;

        public enum Types
        {
            HomeDataAdd,
            HomeDataEdit,
            HomeDataDelete,
            UsersDelete,
            UsersAdmin
        }

        [BindProperty]
        public Types Type { get; set; }

        [BindProperty]
        public InputModelHomeData InputHomeData { get; set; } = default!;

        public class InputModelHomeData
        {
            public string Id { get; set; }
            [Required]
            public string Header { get; set; }
            [Required]
            public string Data { get; set; }
            [Required]
            public string ImageUrl { get; set; }
            [Required]
            public int Order { get; set; }
        }

        [BindProperty]
        public InputModelUser InputUser { get; set; } = default!;

        public class InputModelUser
        {
            public string Id { get; set; }
        }

        public void OnGet()
        {
            InputHomeData = new InputModelHomeData();
            ViewModel = new AdminViewModal();
            ViewModel.HomePage = _context.HomePageData.ToList();
            ViewModel.Users = _appContext.Users.ToList();
            ViewModel.Classes = _context.Classes.Where(r => r.ClassType != "Weekend").OrderBy(r => r.Date).Take(50).ToList();

            foreach(var user in ViewModel.Users)
            {
                user.isAdmin = _userManager.IsInRoleAsync(user, "admin").Result;
            }

            foreach(var classItem in ViewModel.Classes)
            {
                classItem.CurrentSize = _context.ActiveClasses.Where(r => r.ScheduleID == classItem.ScheduleID).Count();
                classItem.Past = classItem.Date < DateTime.Now;
            }

            var largestOrder = ViewModel.HomePage.Max(r => r.Order) + 1;

            InputHomeData.Order = largestOrder;
        }

        public void OnPost()
        {
            if(Type == Types.HomeDataAdd)
            {
                HomePageData homePageData = new HomePageData();
                homePageData.Id = Guid.NewGuid().ToString();
                homePageData.Header = InputHomeData.Header;
                homePageData.Data = InputHomeData.Data;
                homePageData.ImageUrl = InputHomeData.ImageUrl;
                homePageData.Order = InputHomeData.Order;

                _context.HomePageData.Add(homePageData);
                _context.SaveChanges();
            }

            else if(Type == Types.HomeDataEdit)
            {
                var homePageData = _context.HomePageData.Where(r => r.Id == InputHomeData.Id).FirstOrDefault();
                homePageData.Header = InputHomeData.Header;
                homePageData.Data = InputHomeData.Data;
                homePageData.ImageUrl = InputHomeData.ImageUrl;
                homePageData.Order = InputHomeData.Order;

                _context.SaveChanges();
            }

            else if(Type == Types.HomeDataDelete)
            {
                var homePageData = _context.HomePageData.Where(r => r.Id == InputHomeData.Id).FirstOrDefault();
                _context.HomePageData.Remove(homePageData);
                _context.SaveChanges();
            }

            else if(Type == Types.UsersDelete)
            {
                var user = _appContext.Users.Where(r => r.Id == InputUser.Id).FirstOrDefault();
                _appContext.Users.Remove(user);
                _appContext.SaveChanges();
            }

            else if(Type == Types.UsersAdmin)
            {
                var user = _appContext.Users.Where(r => r.Id == InputUser.Id).FirstOrDefault();
                _userManager.AddToRoleAsync(user, "admin").Wait();
            }   



            OnGet();
        }
    }
}
