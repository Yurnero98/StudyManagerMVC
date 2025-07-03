using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Application.Services.Courses;
using MyMvcApp.Domain.Entities;
using MyMvcApp.Infrastructure.Data;
using MyMvcApp.Infrastructure.Repositories;

namespace MyMvcApp.Application.Tests.Services.Courses;

[TestClass]
public class CourseServiceTests
{
    private SqliteConnection _connection = null!;
    private AppDbContext _context = null!;
    private CourseService _courseService = null!;
    private IMapper _mapper = null!;

    [TestInitialize]
    public async Task Setup()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        var courseRepository = new CourseRepository(_context);
        var groupRepository = new GroupRepository(_context);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Course, CourseDto>().ReverseMap();
        });
        _mapper = config.CreateMapper();

        _courseService = new CourseService(courseRepository, _mapper, groupRepository);
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await _connection.DisposeAsync(); 
    }

    [TestMethod]
    public async Task CreateAsync_Should_Add_Course()
    {
        var dto = new CourseDto { Name = "Course 1", Description = "New desc" };
        var id = await _courseService!.CreateAsync(dto);

        var created = await _context!.Courses.FindAsync(id);
        Assert.IsNotNull(created);
        Assert.AreEqual("Course 1", created.Name);
    }

    [TestMethod]
    public async Task GetAsync_Should_Return_Correct_Course()
    {
        var course = new Course { Name = "Course 2", Description = "New desc" };
        _context!.Courses.Add(course);
        await _context.SaveChangesAsync();

        var dto = await _courseService!.GetAsync(course.CourseId);
        Assert.IsNotNull(dto);
        Assert.AreEqual(course.Name, dto!.Name);
    }

    [TestMethod]
    public async Task GetAllAsync_Should_Return_All_Courses()
    {
        _context!.Courses.AddRange(new Course { Name = "Course 1" }, new Course { Name = "Course 2" });
        await _context.SaveChangesAsync();

        var all = await _courseService!.GetAllAsync();
        Assert.AreEqual(2, all.Count);
    }

    [TestMethod]
    public async Task UpdateAsync_Should_Modify_Existing_Course()
    {
        var course = new Course { Name = "OldName", Description = "OldDesc" };
        _context!.Courses.Add(course);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Before update: Id={course.CourseId}, Name={course.Name}, Description={course.Description}");

        var updatedDto = new CourseDto
        {
            CourseId = course.CourseId,
            Name = "NewName",
            Description = "NewDesc"
        };

        var result = await _courseService!.UpdateAsync(updatedDto);
        Console.WriteLine($"UpdateAsync result: {result}");
        Assert.IsTrue(result);

        _context.ChangeTracker.Clear();

        var updated = await _context.Courses.FindAsync(course.CourseId);
        Console.WriteLine($"After update: Id={updated?.CourseId}, Name={updated?.Name}, Description={updated?.Description}");

        Assert.IsNotNull(updated);
        Assert.AreEqual("NewName", updated!.Name);
        Assert.AreEqual("NewDesc", updated.Description);
    }

    [TestMethod]
    public async Task DeleteAsync_Should_Remove_Course()
    {
        var course = new Course { Name = "DeleteMe" };
        _context!.Courses.Add(course);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Before delete: Id={course.CourseId}, Name={course.Name}");

        var result = await _courseService!.DeleteAsync(course.CourseId);
        Console.WriteLine($"DeleteAsync result: {result}");
        Assert.IsTrue(result);

        _context.ChangeTracker.Clear();

        var deleted = await _context.Courses.FindAsync(course.CourseId);
        Console.WriteLine($"After delete: Found={deleted != null}");

        Assert.IsNull(deleted);
    }
}
