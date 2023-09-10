
using Microsoft.AspNetCore.Antiforgery;

namespace MonitorApi
{
    public class AntiforgeryTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;

        public AntiforgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public Task Invoke(HttpContext context)
        {
            Console.WriteLine(context.Request.Path);
            Console.WriteLine("################################################################################################################################");


            if (context.Request.Path.Value.IndexOf("/api", StringComparison.OrdinalIgnoreCase) != -1)
            { 
                var tokens = _antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                    new CookieOptions
                    {
                       SameSite = SameSiteMode.None, 
                        HttpOnly = false,
                        Secure = true

                    }) ;
            }
            return _next(context);
        }
    }

    public static class AntiforgeryTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiforgeryToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AntiforgeryTokenMiddleware>();
        }
    }
}
