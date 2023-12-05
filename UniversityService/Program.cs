using Microsoft.EntityFrameworkCore;
using UniversityService.Application;
using UniversityService.Application.Interfaces;
using UniversityService.Infrastructure;
using UniversityService.Infrastructure.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IDownloadUniversityService, DownloadUniversityService>();
builder.Services.AddScoped<IUniversityRepository, UniversityRepository>();
builder.Services.AddScoped<IUniversityUiLoaderService, UniversityUiLoaderService>();

var hipolabsSourceUrl = builder.Configuration.GetValue<string>("HipolabsSourceUrl")!;
builder.Services.AddHttpClient<IUniversityHttpClient, HipolabsHttpClient>(opt => opt.BaseAddress = new Uri(hipolabsSourceUrl));

var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
builder.Services.AddDbContext<UniversityDbContext>(opt => opt.UseSqlite(connectionString));

builder.Services.AddControllers(options =>
{
    options.AllowEmptyInputInBodyModelBinding = true;
});
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