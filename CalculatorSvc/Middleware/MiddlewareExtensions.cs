using Microsoft.AspNetCore.Builder;

namespace CalculatorSvc.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseIstioTracingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IstioTracingMiddleware>();
        }
    }
}