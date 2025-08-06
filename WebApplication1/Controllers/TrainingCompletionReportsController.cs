using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models.ViewModel;



namespace WebApplication1.Controllers
{
    public class TrainingCompletionReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingCompletionReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Create Report Form
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.VtrIds = new SelectList(_context.GeneralDetails
                .Include(g => g.Trainee)
                .Select(g => new
                {
                    VTRId = g.Trainee.VTRId,
                    Display = g.Trainee.VTRId + " - " + g.FirstName + " " + g.LastName
                }), "VTRId", "Display");

            return View();
        }

        // POST: Create Report
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create(TrainingCompletionCreateViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.VTRId))
                return View(model);

            var gnfc = await _context.GeneralDetails
                .Include(g => g.College)
                .Include(g => g.Department)
                .Include(g => g.Trainee)
                .FirstOrDefaultAsync(g => g.Trainee.VTRId == model.VTRId);

            if (gnfc == null) return NotFound();

            var report = new TrainingCompletionReport
            {
                VTRId = model.VTRId,
                FullName = $"{gnfc.FirstName} {gnfc.MiddleName} {gnfc.LastName}",
                College = gnfc.College?.CollegeName ?? "N/A",
                Course = "B.E.",
                Semester = gnfc.Semester.ToString(),
                Department = gnfc.Department?.Name ?? "N/A",
                TrainingStartDate = new (2025, 01, 20),
                TrainingEndDate = new (2025, 04, 19),
                Behaviour = "Good",
                Progress = "Good",
                CertificateNumber = $"TRG/VTI/2024-25/{new Random().Next(100, 999)}"
            };

            _context.TrainingCompletionReports.Add(report);
            await _context.SaveChangesAsync();

            return RedirectToAction("GeneratePdf", new { id = report.ReportID });
        }

        // GET: List all reports
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrainingCompletionReports.ToListAsync());
        }

        // GET: Edit report
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var report = await _context.TrainingCompletionReports.FindAsync(id);
            if (report == null) return NotFound();
            return View(report);
        }

        // POST: Save edited report
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingCompletionReport report)
        {
            if (id != report.ReportID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TrainingCompletionReports.Any(e => e.ReportID == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
           return RedirectToAction(nameof(Index));
        }

        // GET: Delete report confirmation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var report = await _context.TrainingCompletionReports
                .FirstOrDefaultAsync(r => r.ReportID == id);
            if (report == null) return NotFound();

            return View(report);
        }

        // POST: Confirm delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.TrainingCompletionReports.FindAsync(id);
            if (report != null)
            {
                _context.TrainingCompletionReports.Remove(report);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GeneratePdf(int id)
        {
            var report = await _context.TrainingCompletionReports.FindAsync(id);
            if (report == null) return NotFound();

            return new ViewAsPdf("Certificate", report)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 10, 10, 10),
                FileName = "TrainingCertificate.pdf"
            };
        }

        // GET: Fetch trainee details from VTRId (AJAX)
        [HttpGet]
        public IActionResult GetTraineeDetails(string vtrId)
        {
            var trainee = _context.GeneralDetails
                .Include(g => g.College)
                .Include(g => g.Department)
                .Include(g => g.Trainee)
                .FirstOrDefault(g => g.Trainee.VTRId == vtrId);

            if (trainee == null)
                return Json(new { success = false });

            return Json(new
            {
                fullName = trainee.FirstName + " " + trainee.MiddleName + " " + trainee.LastName,
                college = trainee.College?.CollegeName,
                department = trainee.Department?.Name
            });
        }
    }
}
