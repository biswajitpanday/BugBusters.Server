using OptiOverflow.Api.Helpers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Extension.ConfigureSerilog(builder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAppCorsPolicy();
builder.AddAppDbContext();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
