using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Application.MappingProfiles;
using MyMvcApp.Application.Services.Students;
using MyMvcApp.Domain.Entities;
using MyMvcApp.Infrastructure.Data;
using MyMvcApp.Infrastructure.Repositories;

namespace MyMvcApp.Application.Tests.Services.Students;

[TestClass]
public class StudentServiceTests
{
    private SqliteConnection _connection = null!;
    private AppDbContext _context = null!;
    private StudentService _studentService = null!;
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

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<StudentProfile>();
        });
        _mapper = config.CreateMapper();

        var group = new Group { Name = "TestGroup" };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        var studentRepository = new StudentRepository(_context);
        _studentService = new StudentService(studentRepository, _mapper);
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await _connection.DisposeAsync();
    }

    [TestMethod]
    public async Task CreateAsync_Should_Add_Student()
    {
        var group = await _context.Groups.FirstAsync();
        var dto = new StudentDto { FirstName = "New", LastName = "Student", GroupId = group.GroupId };

        var id = await _studentService.CreateAsync(dto);
        var student = await _context.Students.FindAsync(id);

        Console.WriteLine($"Created Student: Id={student?.StudentId}, Name={student?.FirstName} {student?.LastName}");

        Assert.IsNotNull(student);
        Assert.AreEqual("New", student!.FirstName);
        Assert.AreEqual("Student", student.LastName);
    }

    [TestMethod]
    public async Task GetAsync_Should_Return_Correct_Student()
    {
        var group = await _context.Groups.FirstAsync();
        var student = new Student { FirstName = "New", LastName = "Student", GroupId = group.GroupId };
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        var dto = await _studentService.GetAsync(student.StudentId);
        Console.WriteLine($"Fetched Student: {dto?.FirstName} {dto?.LastName}");

        Assert.IsNotNull(dto);
        Assert.AreEqual("New", dto!.FirstName);
        Assert.AreEqual("Student", dto.LastName);
    }

    [TestMethod]
    public async Task GetAllAsync_Should_Return_All_Students()
    {
        var group = await _context.Groups.FirstAsync();
        _context.Students.AddRange(
            new Student { FirstName = "A", LastName = "B", GroupId = group.GroupId },
            new Student { FirstName = "C", LastName = "D", GroupId = group.GroupId }
        );
        await _context.SaveChangesAsync();

        var all = await _studentService.GetAllAsync();
        Console.WriteLine($"Total Students: {all.Count}");

        Assert.AreEqual(2, all.Count);
    }

    [TestMethod]
    public async Task UpdateAsync_Should_Modify_Student()
    {
        var group = await _context.Groups.FirstAsync();
        var student = new Student { FirstName = "Old", LastName = "Name", GroupId = group.GroupId };
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Before update: Id={student.StudentId}, Name={student.FirstName} {student.LastName}");

        var updatedDto = new StudentDto
        {
            StudentId = student.StudentId,
            FirstName = "New",
            LastName = "Name",
            GroupId = group.GroupId
        };

        var result = await _studentService.UpdateAsync(updatedDto);
        Console.WriteLine($"UpdateAsync result: {result}");
        Assert.IsTrue(result);

        _context.ChangeTracker.Clear();

        var updated = await _context.Students.FindAsync(student.StudentId);
        Console.WriteLine($"After update: Id={updated?.StudentId}, Name={updated?.FirstName} {updated?.LastName}");

        Assert.IsNotNull(updated);
        Assert.AreEqual("New", updated!.FirstName);
        Assert.AreEqual("Name", updated.LastName);
    }

    [TestMethod]
    public async Task DeleteAsync_Should_Remove_Student()
    {
        var group = await _context.Groups.FirstAsync();
        var student = new Student { FirstName = "To", LastName = "Delete", GroupId = group.GroupId };
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Before delete: Id={student.StudentId}, Name={student.FirstName} {student.LastName}");

        var result = await _studentService.DeleteAsync(student.StudentId);
        Console.WriteLine($"DeleteAsync result: {result}");
        Assert.IsTrue(result);

        _context.ChangeTracker.Clear();

        var deleted = await _context.Students.FindAsync(student.StudentId);
        Console.WriteLine($"After delete: Found={deleted != null}");

        Assert.IsNull(deleted);
    }
}
