using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Infrastructure.Repositories;
using Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Register PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("Infrastructure"))); // <-- Set migrations assembly to "Infrastructure"


// Register services
builder.Services.AddScoped<ICourseService, CourseService>();

// Register AutoMapper and scan the current domain for profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
