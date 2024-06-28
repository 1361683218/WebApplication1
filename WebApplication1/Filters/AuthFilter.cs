using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Packaging.Signing;
using WebApplication1.Models;
namespace WebApplication1.Filters
{
	public class AuthFilter : IAuthorizationFilter
	{
		public AuthFilter()
		{
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (!string.IsNullOrEmpty(context.HttpContext.Session.GetString("UserName")) || HasAllowAnonymous(context) == true) return;
			context.Result=new RedirectToActionResult("ShowLogin","Home",null);
		}


		private bool HasAllowAnonymous(AuthorizationFilterContext context)
		{
			var endpoint = context.HttpContext.GetEndpoint();
			if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
			{
				return true;
			}
			return false;
		}
	}

}
