using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Infrastructure.Repositories;
using Application.Mapping;
using System.Text.Json.Serialization;
using Infrastructure.Repositories.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Application.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Domain.Enums;
using Domain.Interfaces;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 7293; // <= This port is 443 in Production!
});

// Add CORS policy FIRST (before other services)
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Your Vite port
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// Register PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("Infrastructure")));


// Register services
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();

builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


// In your API project (e.g., Program.cs or Startup.cs)
builder.Services.AddScoped<IFileService>(provider =>
{
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    var uploadPath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "uploads");

    var db = provider.GetRequiredService<IFileRepository>();
    var encryptionHelper = provider.GetRequiredService<EncryptionHelper>();

    return new FileService(db, uploadPath, encryptionHelper);
});

builder.Services.AddSingleton<EncryptionHelper>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new EncryptionHelper(config);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});



// Register AutoMapper and scan the current domain for profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add other services
//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "APL_Backend", Version = "v1" });
    c.UseInlineDefinitionsForEnums(); // Makes enum values show up clearly
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("ReactFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
