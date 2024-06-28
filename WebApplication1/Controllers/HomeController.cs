using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Diagnostics;
using System.Linq;

namespace WebApplication1.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[AllowAnonymous]
		public IActionResult ShowLogin()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public IActionResult LoginUser(LoginUserModel loginUser)
		{
			_logger.LogInformation($"Login attempt for user: UserName={loginUser.UserName}, UserType={loginUser.UserType}");

			if (loginUser.UserType == "Admin")
			{
				HttpContext.Session.SetString("UserName", loginUser.UserName);
				HttpContext.Session.SetString("UserType", loginUser.UserType);
				return RedirectToAction("ShowAllCourse", "Courses");
			}
			else if (loginUser.UserType == "Student")
			{
				// 尝试将 loginUser.UserName 转换为整数
				if (int.TryParse(loginUser.UserName, out int studentId))
				{
					_logger.LogInformation($"Parsed studentId: {studentId}");
					_logger.LogInformation($"loginUser.PassWord: {loginUser.PassWord}");

					using (var db = new _2109060214DbContext())
					{
						var student = db.Students
							.FirstOrDefault(s => s.Id == studentId && s.InitialPassword == loginUser.PassWord);

						_logger.LogInformation($"Student found: {student != null}");

						if (student != null)
						{
							HttpContext.Session.SetInt32("StudentId", studentId); // 存储 StudentId 作为整数
							HttpContext.Session.SetString("UserName", loginUser.UserName);
							HttpContext.Session.SetString("UserType", loginUser.UserType);
							return RedirectToAction("CourseQuery", "StudentCourse");
						}
					}
				}

				ViewData["Message"] = "Invalid User Name or Password";
				return View("ShowLogin");
			}
			else
			{
				ViewData["Message"] = "Invalid User Type";
				return View("ShowLogin");
			}
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}