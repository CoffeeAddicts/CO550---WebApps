using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitToFight.Models
{
    public class Class
    {
        [Key]
        public Guid ScheduleID { get; set; }
        public string ClassType { get; set; }
        public DateTime Date { get; set; }
        public bool Open { get; set; }
        public int MaxSize { get; set; }
        [NotMapped]
        public int CurrentSize { get; set; }
        [NotMapped]
        public bool Past { get; set; }
        [NotMapped]
        public string Day{ get; set;}
    }
}
