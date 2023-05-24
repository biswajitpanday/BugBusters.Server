using OptiOverflow.Api.Helpers;
using Serilog;
using System.Text.Json.Serialization;
using OptiOverflow.Api.Middleware;
using OptiOverflow.Core.Converters;
using OptiOverflow.Core.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider()))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddResponseCompression();

builder.ConfigureSerilog();
builder.ConfigureAppSwagger();
builder.ConfigureAppCorsPolicy();
builder.ConfigureAppDbContext();
builder.ConfigureAppIdentity();
builder.ConfigureAppAuthentication();
builder.ConfigureAppAutoMapper();

builder.Configuration.GetSection("AppSettings").Get<AppSettings>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors("AppPolicy");
app.UseHttpsRedirection();
app.UseRouting();
app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
