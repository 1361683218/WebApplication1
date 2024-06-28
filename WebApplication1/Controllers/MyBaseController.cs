using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
	public class MyBaseController : Controller
	{
		public IActionResult Index()
		{
			ViewData["UserType"] = HttpContext?.Session.GetString("UserType");//如果不等于NUll，就等于他
			ViewData["UserName"] = HttpContext?.Session.GetString("UserName");
			return View();
		}
	}
}
