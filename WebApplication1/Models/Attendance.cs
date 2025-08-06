using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Display(Name = "Trainee")]
        public int TraineeId { get; set; }

        [ForeignKey("TraineeId")]
        public Trainee Trainee { get; set; }

        public DateTime Date { get; set; }

        public bool IsPresent { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
    }
}
