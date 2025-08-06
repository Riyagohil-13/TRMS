using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Globalization;
using WebApplication1.Models.ViewModel;




namespace WebApplication1.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Attendance/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _context.Departments.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DateTime date, int departmentId)
        {
            var trainees = await _context.GeneralDetails
                .Where(t => t.DepartmentId == departmentId)
                .Include(t => t.Trainee)
                .ToListAsync();

            ViewBag.Date = date;
            ViewBag.DepartmentId = departmentId;
            return View("MarkAttendance", trainees);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(List<Attendance> attendanceList)
        {
            if (attendanceList != null && attendanceList.Any())
            {
                _context.Attendances.AddRange(attendanceList);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var attendance = await _context.Attendances
                .Include(a => a.Trainee)
                    .ThenInclude(t => t.Gnfc)
                        .ThenInclude(g => g.Department)
                .ToListAsync();

            return View(attendance);
        }
        // Monthly Report Page (GET)
        public async Task<IActionResult> MonthlyReport()
        {
            ViewBag.Years = Enumerable.Range(2025, DateTime.Now.Year - 2019).ToList();

            ViewBag.Months = CultureInfo.InvariantCulture.DateTimeFormat.MonthNames
                .Where(m => !string.IsNullOrEmpty(m))
                .Select((m, i) => new SelectListItem
                {
                    Text = m,
                    Value = (i + 1).ToString()
                })
                .ToList();

            return View();
        }

        // Monthly Report Result (POST)
        [HttpPost]
        public async Task<IActionResult> MonthlyReportResult(int month, int year, string vtrId)
        {
            var query = _context.Attendances
                .Include(a => a.Trainee)
                .ThenInclude(t => t.Gnfc)
                .Where(a => a.Date.Month == month && a.Date.Year == year);

            if (!string.IsNullOrEmpty(vtrId))
            {
                query = query.Where(a => a.Trainee.VTRId == vtrId);
            }

            var grouped = await query
                .GroupBy(a => new { a.Trainee.VTRId, a.Trainee.Gnfc.FirstName })
                .Select(g => new MonthlyAttendanceViewModel
                {
                    VTRId = g.Key.VTRId,
                    FirstName = g.Key.FirstName,
                    Dates = g.Select(a => new DailyAttendance
                    {
                        Date = a.Date,
                        IsPresent = a.IsPresent
                    }).OrderBy(d => d.Date).ToList(),
                    TotalPresent = g.Count(a => a.IsPresent),
                    TotalAbsent = g.Count(a => !a.IsPresent),
                    Percentage = (g.Count(a => a.IsPresent) * 100.0) / g.Count()
                }).ToListAsync();

            if (!grouped.Any())
            {
                ViewBag.Message = "No attendance records found for selected criteria.";
            }

            return View("MonthlyReportResult", grouped);
        }



    }
}
