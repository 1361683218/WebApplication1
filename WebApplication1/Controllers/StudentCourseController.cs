using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        // 课程查询
        public IActionResult CourseQuery()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return RedirectToAction("ShowLogin", "Home");
            }

            var courses = _context.Courses.ToList();
            var enrolledCourses = _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Select(e => e.CourseCode)
                .ToList();

            ViewBag.EnrolledCourses = enrolledCourses;
            return View(courses);
        }

        // 选课
        [HttpPost]
        public IActionResult Enroll(string courseCode)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return RedirectToAction("ShowLogin", "Home");
            }

            var course = _context.Courses.FirstOrDefault(c => c.CourseCode == courseCode);
            if (course == null)
            {
                return NotFound();
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId.Value,
                CourseCode = courseCode
            };

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            return RedirectToAction("CourseQuery");
        }

        // 查看已选课程
        public IActionResult MyCourses()
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return RedirectToAction("ShowLogin", "Home");
            }

            var enrollments = _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.CourseCodeNavigation)
                .ToList();

            return View(enrollments);
        }

        // 取消已选课程
        [HttpPost]
        public IActionResult CancelEnrollment(int enrollmentId)
        {
            var studentId = HttpContext.Session.GetInt32("StudentId");
            if (studentId == null)
            {
                return RedirectToAction("ShowLogin", "Home");
            }

            var enrollment = _context.Enrollments.FirstOrDefault(e => e.EnrollmentId == enrollmentId && e.StudentId == studentId);
            if (enrollment == null)
            {
                return NotFound();
            }

            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();

            return RedirectToAction("MyCourses");
        }
    }
}