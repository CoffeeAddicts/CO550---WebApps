namespace FitToFight.Models.ViewModels
{
    public class AdminViewModal
    {
        public List<HomePageData> HomePage { get; set; }
        public List<Class> Classes { get; set; }
        public List<AppSetting> AppSettings{ get; set; }
        public List<ActiveClass> ActiveClasses { get; set; }
        public List<User> Users { get; set; }
    }
}
