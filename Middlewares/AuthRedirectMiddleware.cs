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
            
            // Nếu đang truy cập trang login và đã có role, thì redirect theo vai trò
            if (path == "/account/login")
            {
                var role = context.Session.GetString("Role");

                if (!string.IsNullOrEmpty(role))
                {
                    switch (role)
                    {
                        case "Admin":
                            context.Response.Redirect("/AdminDashboard/Index");
                            return;
                        case "User":
                            context.Response.Redirect("/Home/Index");
                            return;
                        default:
                            // Nếu có role không hợp lệ thì xóa session và cho vào lại login
                            context.Session.Remove("Role");
                            context.Response.Redirect("/Account/Login");
                            return;
                    }
                }
            }

            await _next(context); // tiếp tục xử lý request
        }
    }
}
