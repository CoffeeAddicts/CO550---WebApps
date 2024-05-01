using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitToFight.Models
{
    public class ActiveClass
    {
        [Key]
        public Guid ActiveClassID { get; set; }
        public Guid ScheduleID { get; set; }
        public Guid UserID { get; set; }
        [NotMapped]
        public string Name { get; set; }
    }
}
