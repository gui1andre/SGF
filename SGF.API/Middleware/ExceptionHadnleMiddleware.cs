using System.Text.Json;

namespace SGF.API.Middleware
{
    public class ExceptionHadnleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHadnleMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHadnleMiddleware(RequestDelegate next, ILogger<ExceptionHadnleMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ocorreu um erro inesperado.");
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, title) = e switch
            {
                KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso não localizado."),
                InvalidOperationException => (StatusCodes.Status400BadRequest, "Operação inválida."),
                ArgumentException => (StatusCodes.Status400BadRequest, "Argumento inválido."),
                _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor.")
            };

            context.Response.StatusCode = statusCode;

            var body = new
            {
                title,
                statusCode,
                detail = _env.IsDevelopment() ? e.ToString() : title,
                instance = context.Request.Path.ToString()
            };

            var json = JsonSerializer.Serialize(body);

            await context.Response.WriteAsync(json);
        }
    }
}
