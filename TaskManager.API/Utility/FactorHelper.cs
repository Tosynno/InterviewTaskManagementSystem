namespace TaskManager.API.Utility
{
    public static class FactorHelper
    {
        public static readonly ILoggerFactory dbLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
    }
}
