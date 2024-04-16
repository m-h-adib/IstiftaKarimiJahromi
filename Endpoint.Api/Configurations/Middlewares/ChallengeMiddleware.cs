using Application.Common;

namespace EndPoint.Api.Configurations.Middlewares;

public class ChallengeMiddleware
{
    private readonly RequestDelegate _next;

    public ChallengeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        HostInfo.Url = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.PathBase}";
        HostInfo.Url1 = "https://admin.ketabno.com";

        //System.Threading.Thread.Sleep(5000);


        await _next(httpContext);

        if (httpContext.Response.StatusCode == 401)
        {
            await httpContext.Response.WriteAsJsonAsync(ResultDto.Failed("لطفا وارد سیستم شوید", 401));
        }

		if (httpContext.Response.StatusCode == 403)
		{
			await httpContext.Response.WriteAsJsonAsync(ResultDto.Failed("شما دسترسی به این بخش ندارید", 403));
		}
	}
}
public static class ChallengeMiddlewareExtensions
{
    public static IApplicationBuilder UseChallengeMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ChallengeMiddleware>();
    }
}
