using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class TrainingCompletionReport
    {
        [Key]
        public int ReportID { get; set; }

        [Required]
        public string VTRId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string College { get; set; }

        [Required]
        public string Course { get; set; }

        [Required]
        public string Semester { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TrainingStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TrainingEndDate { get; set; }

        public string Behaviour { get; set; }

        public string Progress { get; set; }

        public string CertificateNumber { get; set; }
    }
}
