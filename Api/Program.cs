using Application;
using Infrastructure;
using FluentValidation.AspNetCore;
using Serilog;
using Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/traceability_log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Sistem baþlatýlýyor...");

    builder.Services.AddControllers();

    builder.Services.AddFluentValidationAutoValidation();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("SqlServer"));

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Uygulama beklenmedik bir þekilde durdu!");
}
finally
{
    Log.CloseAndFlush();
}