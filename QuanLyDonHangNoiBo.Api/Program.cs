using System.Text.Json;
using System.Text.Json.Serialization;
using QuanLyDonHangNoiBo.Application.Abstractions;
using QuanLyDonHangNoiBo.Application.Services;
using QuanLyDonHangNoiBo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddProblemDetails();
builder.Services.AddScoped<OmsApplicationService>();
builder.Services.AddSingleton<IOmsRepository, InMemoryOmsRepository>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
