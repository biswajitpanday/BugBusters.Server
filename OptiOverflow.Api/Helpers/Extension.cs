using Serilog;

namespace OptiOverflow.Api.Helpers;

public static class Extension
{
    public static void ConfigureSerilog()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("serilogconfig.json")
            .Build();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();
    }
}