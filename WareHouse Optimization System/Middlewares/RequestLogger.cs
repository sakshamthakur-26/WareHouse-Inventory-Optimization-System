using System;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WareHouse_Optimization_System.Middlewares
{
    public class RequestLogger
    {
        private readonly RequestDelegate _next;
        private readonly string _logFilePath;

        public RequestLogger(RequestDelegate next)
        {
            _next = next;
            var logsDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }
            _logFilePath = Path.Combine(logsDir, "requests.log");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var now = DateTime.Now;
            var method = httpContext.Request?.Method ?? "UNKNOWN";
            var path = httpContext.Request?.Path.Value ?? string.Empty;
            var line = $"{now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)} | {method} | {path}{Environment.NewLine}";
            await File.AppendAllTextAsync(_logFilePath, line);
            await _next(httpContext);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogger>();
        }
    }
}
