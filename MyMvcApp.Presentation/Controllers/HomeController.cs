using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Application.Services.Courses;
using MyMvcApp.Application.Services.Groups;
using MyMvcApp.Application.Services.Students;
using MyMvcApp.Application.DTOs;

namespace MyMvcApp.Presentation.Controllers;

public class HomeController(ICourseService courseService, IGroupService groupService, IStudentService studentService) : Controller
{
    private readonly ICourseService _courseService = courseService;
    private readonly IGroupService _groupService = groupService;
    private readonly IStudentService _studentService = studentService;

    public async Task<IActionResult> Index()
    {
        var courses = await _courseService.GetAllAsync();
        return View(courses); 
    }

    public async Task<IActionResult> Groups(int courseId)
    {
        var course = await _courseService.GetAsync(courseId);
        if (course == null) return NotFound();

        var groups = await _groupService.GetByCourseIdAsync(courseId);

        var viewModel = new CourseWithGroupsDto
        {
            Id = course.CourseId,
            Name = course.Name,
            Groups = groups.ToList()
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Students(int groupId)
    {
        var group = await _groupService.GetAsync(groupId);
        if (group == null) return NotFound();

        var students = await _studentService.GetByGroupIdAsync(groupId);

        var viewModel = new GroupWithStudentsDto
        {
            Id = group.GroupId,
            Name = group.Name,
            Students = students.ToList()
        };

        return View(viewModel);
    }
}
