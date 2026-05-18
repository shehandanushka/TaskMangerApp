using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Auth;
using TaskManagerAPI.Data;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Repositories.Interfaces;
using TaskManagerAPI.Services;
using TaskManagerAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// =============================================
// SERVICES
// =============================================

builder.Services.AddControllers();

// ✅ REQUIRED for Swagger to discover endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository DI
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Service DI
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Authentication
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>(
        "BasicAuthentication", null);

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// =============================================
// PIPELINE
// =============================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
        c.RoutePrefix = string.Empty; // 👈 opens Swagger at root (https://localhost:xxxx/)
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Auto-migrate
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();