using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlite("Data Source=Data.db")
    );

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovieContext>();
    // dbContext.Database.EnsureCreated(); // Create database if it doesn't exist

    // // Seed data
    // if (!dbContext.Movies.Any())
    // {
    //     dbContext.Movies.AddRange(
    //         new Movie { Title = "Inception", Year = 2010, Director = "Christopher Nolan" },
    //         new Movie { Title = "Interstellar", Year = 2014, Director = "Christopher Nolan" },
    //         new Movie { Title = "The Dark Knight", Year = 2008, Director = "Christopher Nolan" },
    //         new Movie { Title = "Parasite", Year = 2019, Director = "Bong Joon-ho" }
    //     );
    //     dbContext.SaveChanges();
    // }
}
app.UseCors("AllowReactApp");
app.MapControllers();
app.Run();