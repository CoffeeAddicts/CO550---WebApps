using FitToFight.Models;
using FitToFight.Models.ViewModels;
using FitToFight.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            UsersAdmin,
            UsersAdminRemove,
            ClassesCancel,
            ClassesUnCancel,
            AppSettingsEdit
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
            [DataType(DataType.MultilineText)]
            public string Data { get; set; }
            public string ImageUrl { get; set; } = "";
            [Required]
            public int Order { get; set; }
        }

        [BindProperty]
        public InputModelUser InputUser { get; set; } = default!;

        public class InputModelUser
        {
            public string Id { get; set; }
        }

        [BindProperty]
        public InputModelClass InputClass { get; set; } = default!;

        public class InputModelClass
        {
            public string Id { get; set; }
        }

        [BindProperty]

        public InputModelAppSetting InputAppSettings { get; set; } = default!;

        public class InputModelAppSetting : AppSetting
        {
        }

        public void OnGet()
        {
            InputHomeData = new InputModelHomeData();
            ViewModel = new AdminViewModal();
            ViewModel.HomePage = _context.HomePageData.ToList();
            ViewModel.Users = _appContext.Users.ToList();
            ViewModel.Classes = _context.Classes.Where(r => r.ClassType != "Weekend").OrderBy(r => r.Date).Take(50).ToList();
            ViewModel.ActiveClasses = _context.ActiveClasses.ToList();
            ViewModel.AppSettings = _context.AppSettings.ToList();

            foreach (var activeClass in ViewModel.ActiveClasses)
            {
                activeClass.Name = _appContext.Users.Where(r => r.Id == activeClass.UserID.ToString()).Select(r => $"{r.FirstName} {r.LastName}").FirstOrDefault() ?? "";
            }

            foreach (var user in ViewModel.Users)
            {
                user.isAdmin = _userManager.IsInRoleAsync(user, "admin").Result;
            }

            foreach (var classItem in ViewModel.Classes)
            {
                classItem.CurrentSize = _context.ActiveClasses.Where(r => r.ScheduleID == classItem.ScheduleID).Count();
                classItem.Past = classItem.Date < DateTime.Now;
                classItem.Day = classItem.Date.DayOfWeek.ToString();
            }

            var largestOrder = ViewModel.HomePage.Max(r => r.Order) + 1;

            InputHomeData.Order = largestOrder;
        }

        public void OnPost()
        {
            if (Type == Types.HomeDataAdd)
            {
                if (InputHomeData.Header == "")
                {
                    OnGet();
                    return;
                }
                HomePageData homePageData = new HomePageData();
                homePageData.Id = Guid.NewGuid().ToString();
                homePageData.Header = InputHomeData.Header ?? "";
                homePageData.Data = InputHomeData.Data ?? "";
                homePageData.ImageUrl = InputHomeData.ImageUrl ?? "";
                homePageData.Order = InputHomeData.Order;

                _context.HomePageData.Add(homePageData);
                _context.SaveChanges();
            }

            else if (Type == Types.HomeDataEdit)
            {
                var homePageData = _context.HomePageData.Where(r => r.Id == InputHomeData.Id).FirstOrDefault();
                homePageData.Header = InputHomeData.Header;
                homePageData.Data = InputHomeData.Data;
                homePageData.ImageUrl = InputHomeData.ImageUrl ?? "";
                homePageData.Order = InputHomeData.Order;

                _context.SaveChanges();
            }

            else if (Type == Types.HomeDataDelete)
            {
                var homePageData = _context.HomePageData.Where(r => r.Id == InputHomeData.Id).FirstOrDefault();
                _context.HomePageData.Remove(homePageData);
                _context.SaveChanges();
            }

            else if (Type == Types.UsersDelete)
            {
                var user = _appContext.Users.Where(r => r.Id == InputUser.Id).FirstOrDefault();
                _appContext.Users.Remove(user);
                _appContext.SaveChanges();
            }

            else if (Type == Types.UsersAdmin)
            {
                var user = _appContext.Users.Where(r => r.Id == InputUser.Id).FirstOrDefault();
                _userManager.AddToRoleAsync(user, "admin").Wait();
            }
            else if (Type == Types.UsersAdminRemove)
            {
                var user = _appContext.Users.Where(r => r.Id == InputUser.Id).FirstOrDefault();
                _userManager.RemoveFromRoleAsync(user, "admin").Wait();
            }

            else if (Type == Types.ClassesCancel)
            {
                var allActiveClasses = _context.ActiveClasses.Where(r => r.ScheduleID == Guid.Parse(InputClass.Id)).ToList();
                foreach (var activeClass in allActiveClasses)
                {
                    _context.ActiveClasses.Remove(activeClass);
                }
                _context.SaveChanges();
                var classItem = _context.Classes.Where(r => r.ScheduleID == Guid.Parse(InputClass.Id)).FirstOrDefault();
                classItem.Open = false;
                _context.SaveChanges();
            }

            else if (Type == Types.ClassesUnCancel)
            {
                var classItem = _context.Classes.Where(r => r.ScheduleID == Guid.Parse(InputClass.Id)).FirstOrDefault();
                classItem.Open = true;
                _context.SaveChanges();
            }

            else if (Type == Types.AppSettingsEdit)
            {
                var appSetting = _context.AppSettings.Where(r => r.Key == InputAppSettings.Key).FirstOrDefault();
                appSetting.ValueString = InputAppSettings.ValueString;
                appSetting.ValueInt = InputAppSettings.ValueInt;
                appSetting.ValueBool = InputAppSettings.ValueBool;
                appSetting.ValueDecimal = InputAppSettings.ValueDecimal;
                appSetting.Explanation = InputAppSettings.Explanation;
                _context.SaveChanges();
            }



            OnGet();
        }
    }
}
