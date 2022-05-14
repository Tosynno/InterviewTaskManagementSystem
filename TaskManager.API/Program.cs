using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.BusinessLogic;
using TaskManager.API.Extensions;
using TaskManager.API.Utility;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddScoped<AppDbContext>();
//Async DB
builder.Services.AddPooledDbContextFactory<AppDbContext>(option => option.UseLoggerFactory(FactorHelper.dbLoggerFactory).UseSqlServer(configuration.GetConnectionString("DefualtConnection")));
builder.Services.AddTransient<GeneralClass>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
