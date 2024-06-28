using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CoursesController : Controller
    {
        private readonly _2109060214DbContext _context;

        public CoursesController(_2109060214DbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCourse()
        {
            return View();
        }

        public async Task<IActionResult> ShowAllCourse()
        {
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            List<Course> courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        // GET: Courses/CourseEdit/5
        public async Task<IActionResult> CourseEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/CourseEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourseEdit(string id, [Bind("CourseCode,CourseName,Credits")] Course course)
        {
            if (id != course.CourseCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseCode))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ShowAllCourse));
            }
            return View(course);
        }

        public async Task<IActionResult> CourseDelete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            if (await HasStudentsEnrolled(id))
            {
                TempData["ErrorMessage"] = "Cannot delete course because there are students enrolled in it.";
                return RedirectToAction(nameof(ShowAllCourse));
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ShowAllCourse));
        }

        // POST: Courses/AddCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse([Bind("CourseCode,CourseName,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                if (CourseExists(course.CourseCode))
                {
                    ModelState.AddModelError("CourseCode", "Course code already exists.");
                    return View(course);
                }

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ShowAllCourse));
            }
            return View(course);
        }

        private bool CourseExists(string id)
        {
            return _context.Courses.Any(e => e.CourseCode == id);
        }

        private async Task<bool> HasStudentsEnrolled(string courseCode)
        {
            // Assuming you have a relationship between Course and Student through an Enrollment table
            return await _context.Enrollments.AnyAsync(e => e.CourseCode == courseCode);
        }
    }
}