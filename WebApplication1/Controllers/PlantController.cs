﻿using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Buffers;

using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PlantController : Controller
    {
        private readonly ApplicationDbContext context;

        public PlantController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var student = await context.Students.ToListAsync();
        //    return View(student);
        //}
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // Define sorting parameters
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["CollegeSortParam"] = sortOrder == "college" ? "college_desc" : "college";
            var students = context.Plants
                    .AsQueryable();
            // Apply search filter if search string is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString));
            }

            // Apply sorting based on the provided sort order
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Name);
                    break;
                case "id":
                    students = students.OrderBy(s => s.Id);
                    break;
                case "id_desc":
                    students = students.OrderByDescending(s => s.Id);
                    break;

                default:
                    students = students.OrderBy(s => s.Name);
                    break;
            }
            return View(await students.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plant gnfc)
        {
            var gnfcId = new Plant
            {
                Id = gnfc.Id,
                Name = gnfc.Name,
            };
            await context.Plants.AddAsync(gnfcId);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Plant");
        }
        [HttpGet]

        public async Task<IActionResult> Update(int id)
        {

            var student = await context.Plants.FindAsync(id);
            return View(student);

        }
        [HttpPost]

        public async Task<IActionResult> Update(Department viewModel)
        {
            var student = await context.Plants.FindAsync(viewModel.Id);
            if (student is not null)
            {
                //student.Id = viewModel.VTRId;
                student.Name = viewModel.Name;
            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Plant");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await context.Plants.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await context.Plants.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            context.Plants.Remove(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Department"); // Redirect to the list page
        }
    }
}

