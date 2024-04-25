using System.ComponentModel.DataAnnotations;

namespace FitToFight.Models
{
    public class Class
    {
        [Key]
        public Guid ScheduleID { get; set; }
        public string ClassType { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public bool Open { get; set; }
        public int MaxSize { get; set; }
    }
}
