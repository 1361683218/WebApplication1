using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class StudentsController : Controller
	{
		private readonly _2109060214DbContext _context;

		public StudentsController(_2109060214DbContext context)
		{
			_context = context;
		}

		// GET: Students/ShowAllStudent
		public async Task<IActionResult> ShowAllStudent()
		{
			List<Student> students = await _context.Students.ToListAsync();
			return View(students);
		}

		// GET: Students/AddStudent
		public IActionResult AddStudent()
		{
			return View();
		}

		// POST: Students/AddStudent
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddStudent([Bind("Id,Name,ClassName,InitialPassword")] Student student)
		{
			if (ModelState.IsValid)
			{
				if (_context.Students.Any(s => s.Id == student.Id))
				{
					ModelState.AddModelError("Id", "Student ID already exists.");
					return View(student);
				}

				_context.Add(student);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(ShowAllStudent));
			}
			return View(student);
		}

		// GET: Students/StudentEdit/5
		public async Task<IActionResult> StudentEdit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var student = await _context.Students.FindAsync(id);
			if (student == null)
			{
				return NotFound();
			}
			return View(student);
		}

		// POST: Students/StudentEdit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> StudentEdit(int id, [Bind("Id,Name,ClassName,InitialPassword")] Student student)
		{
			if (id != student.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(student);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!StudentExists(student.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(ShowAllStudent));
			}
			return View(student);
		}

		// GET: Students/DeleteStudent/5
		public async Task<IActionResult> DeleteStudent(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var student = await _context.Students
				.FirstOrDefaultAsync(m => m.Id == id);
			if (student == null)
			{
				return NotFound();
			}

			return View(student);
		}

		// POST: Students/DeleteStudent/5
		[HttpPost, ActionName("DeleteStudent")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var student = await _context.Students.FindAsync(id);
			if (student != null)
			{
				// Delete associated enrollments
				var enrollments = await _context.Enrollments.Where(e => e.StudentId == student.Id).ToListAsync();
				_context.Enrollments.RemoveRange(enrollments);

				_context.Students.Remove(student);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(ShowAllStudent));
		}

		private bool StudentExists(int id)
		{
			return _context.Students.Any(e => e.Id == id);
		}
	}
}