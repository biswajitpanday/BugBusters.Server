using Serilog;
using System.Text.Json.Serialization;
using BugBusters.Server.Api.Helpers;
using BugBusters.Server.Api.Middleware;
using BugBusters.Server.Core.Converters;
using BugBusters.Server.Core.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider()))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddResponseCompression();

builder.ConfigureAppComponents();
builder.ConfigureDi();
builder.Configuration.GetSection("AppSettings").Get<AppSettings>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseRequestResponseLogging();
    app.SeedData();
}

app.UseSerilogRequestLogging();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseRouting();
app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
