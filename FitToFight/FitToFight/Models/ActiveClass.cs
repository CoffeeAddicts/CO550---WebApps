using System.ComponentModel.DataAnnotations;

namespace FitToFight.Models
{
    public class ActiveClass
    {
        [Key]
        public Guid ActiveClassID { get; set; }
        public Guid ScheduleID { get; set; }
        public Guid UserID { get; set; }
    }
}
