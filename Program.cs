using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContextPool<AppDbContext>(options =>
                 options.UseMySql(mySqlConnection,
                       ServerVersion.AutoDetect(mySqlConnection)));

// Add services to the container.

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
