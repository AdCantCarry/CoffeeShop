using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoffeeShop.Middlewares
{
    public class AuthRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path == "/account/login")
            {
                var role = context.Session.GetString("Role");

                if (role == "Admin")
                {
                    context.Response.Redirect("/AdminDashboard/Index");
                    return;
                }

                if (role == "User")
                {
                    context.Response.Redirect("/Home/Index");
                    return;
                }
            }

            await _next(context); // tiếp tục xử lý request
        }
    }
}
