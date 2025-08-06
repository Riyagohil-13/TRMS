public class AttendanceReportViewModel
{
    public string VTRId { get; set; }
    public string FirstName { get; set; }
    public int TotalPresent { get; set; }
    public int TotalAbsent { get; set; }
    public double AttendancePercentage => TotalPresent + TotalAbsent > 0
        ? Math.Round((double)TotalPresent / (TotalPresent + TotalAbsent) * 100, 2)
        : 0;
}
