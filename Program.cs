using Microsoft.EntityFrameworkCore;
using TimescaleApi.Data;
using TimescaleApi.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
    .EnableSensitiveDataLogging() // для dev
    .EnableDetailedErrors()); // для dev

builder.Services.AddScoped<ICsvService, CsvService>();

// builder.Services.AddScoped<ICsvProcessingService, CsvProcessingService>();
// builder.Services.AddScoped<IValueRepository, ValueRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
