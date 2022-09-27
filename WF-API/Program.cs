using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WF_API.Data;
using WF_API.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WF_APIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WF_APIContext") ?? throw new InvalidOperationException("Connection string 'WF_APIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<WF_APIContext>(opt =>
    opt.UseInMemoryDatabase("Forecast"));
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "TodoApi", Version = "v1" });
//});
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
