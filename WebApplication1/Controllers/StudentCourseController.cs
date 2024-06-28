using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentCourseController : Controller
    {
        private readonly _2109060214DbContext _context;

        public StudentCourseController(_2109060214DbContext context)
        {
            _context = context;
        }

        // GET: StudentCourse/CourseQuery
        public async Task<IActionResult> CourseQuery(string courseCode, string courseName, int? credits)
        {
            var courses = _context.Courses.AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(courseCode))
            {
                courses = courses.Where(c => c.CourseCode.Contains(courseCode));
            }

            if (!string.IsNullOrEmpty(courseName))
            {
                courses = courses.Where(c => c.CourseName.Contains(courseName));
            }

            if (credits.HasValue)
            {
                courses = courses.Where(c => c.Credits == credits.Value);
            }

            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId.HasValue)
            {
                var enrolledCourses = await _context.Enrollments
                    .Where(e => e.StudentId == studentId.Value)
                    .Select(e => e.CourseCode)
                    .ToListAsync();

                ViewBag.EnrolledCourses = enrolledCourses;
            }

            var result = await courses.ToListAsync();
            return View(result);
        }

        // POST: StudentCourse/SelectCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectCourse(string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode))
            {
                return NotFound();
            }

            int? studentIdNullable = HttpContext.Session.GetInt32("StudentId");

            if (!studentIdNullable.HasValue)
            {
                TempData["ErrorMessage"] = "Student ID not found. Please log in again.";
                return RedirectToAction("ShowLogin"); // Assuming you have a login action
            }

            int studentId = studentIdNullable.Value;

            if (await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseCode == courseCode))
            {
                TempData["ErrorMessage"] = "You have already selected this course.";
                return RedirectToAction(nameof(CourseQuery));
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseCode = courseCode
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Course selected successfully.";
            return RedirectToAction(nameof(MyCourses));
        }

        // GET: StudentCourse/MyCourses
        public async Task<IActionResult> MyCourses()
        {
            int? studentIdNullable = HttpContext.Session.GetInt32("StudentId");

            if (!studentIdNullable.HasValue)
            {
                TempData["ErrorMessage"] = "Student ID not found. Please log in again.";
                return RedirectToAction("ShowLogin"); // Assuming you have a login action
            }

            int studentId = studentIdNullable.Value;

            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.CourseCodeNavigation)
                .ToListAsync();

            return View(enrollments);
        }

        // POST: StudentCourse/CancelCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelCourse(string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode))
            {
                return NotFound();
            }

            int? studentIdNullable = HttpContext.Session.GetInt32("StudentId");

            if (!studentIdNullable.HasValue)
            {
                TempData["ErrorMessage"] = "Student ID not found. Please log in again.";
                return RedirectToAction("ShowLogin"); // Assuming you have a login action
            }

            int studentId = studentIdNullable.Value;

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseCode == courseCode);

            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Course not found in your enrollments.";
            }

            return RedirectToAction(nameof(MyCourses));
        }

        [HttpGet]
        public IActionResult GetCourses()
        {
            var courses = _context.Courses.ToList();
            return Json(new { data = courses });
        }
    }
}