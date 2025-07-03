using Microsoft.EntityFrameworkCore;
using MyMvcApp.Infrastructure.Data;
using MyMvcApp.Application.Services.Courses;
using MyMvcApp.Application.Services.Groups;
using MyMvcApp.Application.Services.Students;
using MyMvcApp.Domain.Repositories;
using MyMvcApp.Infrastructure.Repositories;
using MyMvcApp.Application.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(CourseProfile).Assembly);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
