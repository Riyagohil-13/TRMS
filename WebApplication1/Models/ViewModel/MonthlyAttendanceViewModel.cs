namespace WebApplication1.Models.ViewModel
{
    public class MonthlyAttendanceViewModel
    {
        public string VTRId { get; set; }
        public string FirstName { get; set; }
        public List<DailyAttendance> Dates { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public double Percentage { get; set; }
    }

    public class DailyAttendance
    {
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}
