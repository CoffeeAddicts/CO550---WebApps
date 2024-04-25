using System.ComponentModel.DataAnnotations;

namespace FitToFight.Models
{
    public class HomePageData
    {
        [Key]
        public string Key { get; set; }
        public string Data { get; set; }
    }
}
