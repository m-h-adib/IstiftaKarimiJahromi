using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Application.Common;

public class MyAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
	readonly string[] _requiredClaims;
	public MyAuthorizeAttribute(params string[] claims)
    {
		_requiredClaims = claims;
	}
   

	public async void OnAuthorization(AuthorizationFilterContext context)
	{
		var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
		if (!isAuthenticated)
		{
			context.Result = new UnauthorizedResult();
			return;
		}

		var hasAllRequredClaims = context.HttpContext.User.Claims.Any(p => p.Type == ClaimTypes.Role && p.Value == "Admin");
		//var hasAllRequredClaims = _requiredClaims.All(claim => context.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.Role && x.Value=="Admin"));
		if (!hasAllRequredClaims)
		{
			context.Result = new ForbidResult();
			return;
		}
	}
}
