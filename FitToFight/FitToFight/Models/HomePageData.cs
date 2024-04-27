using System.ComponentModel.DataAnnotations;

namespace FitToFight.Models
{
    public class HomePageData
    {
        [Key]
        public string Id { get; set; }
        public string Header { get; set; }
        public string Data { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
