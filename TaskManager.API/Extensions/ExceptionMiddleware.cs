using NLog;
using System.Net;
using System.Text.Json;

namespace TaskManager.API.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _Logger;
        private readonly IHostEnvironment _env;
        Logger logger = LogManager.GetCurrentClassLogger();

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _Logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //Read Configuration from appSettings    
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //Initialize Logger    
            //  Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, ex.Message);
                logger.Debug(ex.Source, ex.Message);
                //   Log.Error(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;


                var metadate = new
                {
                    StatusCodes = context.Response.StatusCode,
                    Details = ex.StackTrace?.ToString(),
                    Errormessage = ex.Message?.ToString(),
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(metadate, options);


                await context.Response.WriteAsync(json);
            }
        }
    }
}
