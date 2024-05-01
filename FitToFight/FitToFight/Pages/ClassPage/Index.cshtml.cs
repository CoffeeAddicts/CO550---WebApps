using FitToFight.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace FitToFight.Pages.ClassPage
{
    public class IndexModel : PageModel
    {
        private readonly FitToFightContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(FitToFightContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //Need to also make sure there are days before 
        public List<Class> ClassesView { get; set; } = default!;
        public List<ActiveClass> ActiveClasses { get; set; }

        public void OnGet()
        {
            var appSettings = _context.AppSettings.ToList();

            // Here the first thing will be to check that there is a years worth of data in the database
            //If not then we will need to create a years worth of data

            List<Class> classes = new List<Class>();

            if (_context.Classes != null)
            {
                classes = _context.Classes.OrderBy(r => r.Date).ToList();
            }


            var lastDate = DateTime.Now;

            var keepingLogs = appSettings.Where(r => r.Key == "DaysToKeepLogs").Select(r => r.ValueInt).FirstOrDefault() ?? 30;
            var kidsMaxCapacity = appSettings.Where(r => r.Key == "KidsMaxCapacity").Select(r => r.ValueInt).FirstOrDefault() ?? 10;
            var normalMaxCapacity = appSettings.Where(r => r.Key == "NormalMaxCapacity").Select(r => r.ValueInt).FirstOrDefault() ?? 10;
            var ladiesMaxCapacity = appSettings.Where(r => r.Key == "LadiesMaxCapacity").Select(r => r.ValueInt).FirstOrDefault() ?? 10;

            var daysInfront = 100;

            foreach (var classItem in classes)
            {
                if (classItem.Date < DateTime.Now.AddDays(-keepingLogs))
                {
                    _context.Classes.Remove(classItem);
                    _context.SaveChanges();
                }
            }


            if (classes.Count() != 0)
            {
                lastDate = classes.OrderBy(r => r.Date).Last().Date;
            }
            else
            {
                lastDate = lastDate.Date.AddDays(-keepingLogs);
            }
            while (lastDate < DateTime.Now.AddDays(daysInfront))
            {
                TimeSpan time = new TimeSpan(0, 0, 0);
                lastDate = lastDate.Date + time;
                //Add a class for each day

                TimeSpan adultLesson = new TimeSpan(19, 0, 0);
                TimeSpan kidsLesson = new TimeSpan(17, 30, 0);


                if (classes.Select(r => r.Date).Contains(lastDate))
                {
                    continue;
                }

                var classType = GetClassBasedOnDay(lastDate);
                if (classType == "Normal")
                {
                    var kidsClass = new Class()
                    {
                        ScheduleID = Guid.NewGuid(),
                        ClassType = "Kids",
                        Date = lastDate + kidsLesson,
                        Open = true,
                        MaxSize = kidsMaxCapacity
                    };
                    _context.Classes.Add(kidsClass);
                    _context.SaveChanges();
                }
                var newClass = new Class()
                {
                    ScheduleID = Guid.NewGuid(),
                    ClassType = classType,
                    Date = lastDate + adultLesson,
                    Open = classType != "Weekend",
                    MaxSize = classType == "Ladies" ? ladiesMaxCapacity : normalMaxCapacity
                };
                _context.Classes.Add(newClass);
                _context.SaveChanges();
                lastDate = lastDate.AddDays(1);
            }

            foreach (var classItem in classes)
            {
                classItem.CurrentSize = _context.ActiveClasses.Where(r => r.ScheduleID == classItem.ScheduleID).Count();
            }

            ClassesView = classes;
            ActiveClasses = _context.ActiveClasses.ToList();
        }
        [BindProperty]
        public string scheduleId { get; set; } = "";

        [BindProperty]
        public string type { get; set; } = "";
        public void OnPost()
        {
            var classID = Guid.Parse(scheduleId);
            var classItem = _context.Classes.Where(r => r.ScheduleID == classID).FirstOrDefault();

            var task = _userManager.GetUserAsync(User);
            task.Wait();
            var appUser = task.Result;

            if (type == "Cancel")
            {
                var classToRemove = _context.ActiveClasses.Where(r => r.ScheduleID == classItem.ScheduleID && r.UserID == Guid.Parse(appUser.Id)).FirstOrDefault();
                _context.ActiveClasses.Remove(classToRemove);
                _context.SaveChanges();
                OnGet();
                return;
            }

            if (_context.ActiveClasses.Where(r => r.ScheduleID == classItem.ScheduleID && r.UserID == Guid.Parse(appUser.Id)).Count() != 0)
            {
                OnGet();
                return;
            }

            var activeClass = new ActiveClass()
            {
                ActiveClassID = Guid.NewGuid(),
                ScheduleID = classItem.ScheduleID,
                UserID = Guid.Parse(appUser.Id)
            };
            _context.ActiveClasses.Add(activeClass);
            _context.SaveChanges();



            OnGet();
        }


        public string GetClassBasedOnDay(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Monday || date.DayOfWeek == DayOfWeek.Tuesday || date.DayOfWeek == DayOfWeek.Thursday)
            {
                return "Normal";
            }
            else if (date.DayOfWeek == DayOfWeek.Wednesday || date.DayOfWeek == DayOfWeek.Friday)
            {
                return "Ladies";
            }
            else
            {
                return "Weekend";
            }
        }
    }
}
