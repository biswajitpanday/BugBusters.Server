using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace OptiOverflow.Api.Helpers;

public static class Extension
{
    public static void ConfigureSerilog(WebApplicationBuilder builder)
    {
        try
        {
            const string logOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] \t{Message}{NewLine:1}{Exception:1}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: logOutputTemplate)
                .WriteTo.File(new MessageTemplateTextFormatter(outputTemplate: logOutputTemplate), 
                    "Logs/log.txt", 
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    shared:true
                )
                .Enrich.WithProperty("ApplicationName", "OptiOverflow")
                .CreateLogger();
            builder.Host.UseSerilog();
            Log.Logger.Information("Serilog Configured Successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting up Serilog: {ex}");
        }
    }
}