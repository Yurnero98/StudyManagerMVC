using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Application.Services.Groups;
using MyMvcApp.Domain.Entities;
using MyMvcApp.Infrastructure.Data;
using MyMvcApp.Infrastructure.Repositories;

namespace MyMvcApp.Application.Tests.Services.Groups;

[TestClass]
public class GroupServiceTests
{
    private SqliteConnection _connection = null!;
    private AppDbContext _context = null!;
    private GroupService _groupService = null!;
    private IMapper? _mapper;

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

        var groupRepository = new GroupRepository(_context);
        var studentRepository = new StudentRepository(_context);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Group, GroupDto>().ReverseMap();
        });
        _mapper = config.CreateMapper();

        _groupService = new GroupService(groupRepository, _mapper, studentRepository);
    }

    [TestCleanup]
    public async Task Cleanup()
    {
        await _connection.DisposeAsync();
    }

    [TestMethod]
    public async Task CreateAsync_Should_Add_Group()
    {
        var course = new Course { Name = "Course for Group" };
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        var dto = new GroupDto { Name = "Group 1", CourseId = course.CourseId };
        var id = await _groupService.CreateAsync(dto);

        var created = await _context.Groups.FindAsync(id);
        Assert.IsNotNull(created);
        Assert.AreEqual("Group 1", created.Name);
    }

    [TestMethod]
    public async Task GetAsync_Should_Return_Correct_Group()
    {
        var group = new Group { Name = "Group 2" };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        var dto = await _groupService.GetAsync(group.GroupId);
        Assert.IsNotNull(dto);
        Assert.AreEqual(group.Name, dto!.Name);
    }

    [TestMethod]
    public async Task GetAllAsync_Should_Return_All_Groups()
    {
        _context.Groups.AddRange(new Group { Name = "Group A" }, new Group { Name = "Group B" });
        await _context.SaveChangesAsync();

        var all = await _groupService.GetAllAsync();
        Assert.AreEqual(2, all.Count);
    }

    [TestMethod]
    public async Task UpdateAsync_Should_Modify_Existing_Group()
    {
        var group = new Group { Name = "OldGroup" };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Before update: Id={group.GroupId}, Name={group.Name}");

        var updatedDto = new GroupDto
        {
            GroupId = group.GroupId,
            Name = "UpdatedGroup",
            CourseId = group.CourseId
        };

        var result = await _groupService.UpdateAsync(updatedDto);
        Console.WriteLine($"UpdateAsync result: {result}");
        Assert.IsTrue(result);

        _context.ChangeTracker.Clear();

        var updated = await _context.Groups.FindAsync(group.GroupId);
        Console.WriteLine($"After update: Id={updated?.GroupId}, Name={updated?.Name}");

        Assert.IsNotNull(updated);
        Assert.AreEqual("UpdatedGroup", updated!.Name);
    }

    [TestMethod]
    public async Task DeleteAsync_Should_Remove_Group()
    {
        var group = new Group { Name = "DeleteMe" };
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        Console.WriteLine($"Before delete: Id={group.GroupId}, Name={group.Name}");

        var result = await _groupService.DeleteAsync(group.GroupId);
        Console.WriteLine($"DeleteAsync result: {result}");
        Assert.IsTrue(result);

        _context.ChangeTracker.Clear();

        var deleted = await _context.Groups.FindAsync(group.GroupId);
        Console.WriteLine($"After delete: Found={deleted != null}");

        Assert.IsNull(deleted);
    }
}
