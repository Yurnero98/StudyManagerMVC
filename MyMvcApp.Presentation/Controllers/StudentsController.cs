using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyMvcApp.Application.Services.Students;
using MyMvcApp.Application.Services.Groups;
using MyMvcApp.Presentation.ViewModels.StudentViewModels;

namespace MyMvcApp.Presentation.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentService _studentService;
    private readonly IGroupService _groupService;

    public StudentsController(IStudentService studentService, IGroupService groupService)
    {
        _studentService = studentService;
        _groupService = groupService;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _studentService.GetAllAsync();
        var groups = await _groupService.GetAllAsync();

        var vm = new StudentIndexViewModel
        {
            Students = students,
            Groups = groups.ToDictionary(g => g.GroupId, g => g.Name)
        };

        return View(vm);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new StudentEditViewModel
        {
            Groups = await GetGroupsSelectListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentEditViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Groups = await GetGroupsSelectListAsync();
            return View(vm);
        }

        await _studentService.CreateAsync(vm.Student!);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var student = await _studentService.GetAsync(id);
        if (student == null) return NotFound();

        var vm = new StudentEditViewModel
        {
            Student = student,
            Groups = await GetGroupsSelectListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(StudentEditViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Groups = await GetGroupsSelectListAsync();
            return View(vm);
        }

        var updated = await _studentService.UpdateAsync(vm.Student!);
        if (!updated) return NotFound();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var student = await _studentService.GetAsync(id);
        if (student == null) return NotFound();

        var groups = await _groupService.GetAllAsync();
        var groupName = groups.FirstOrDefault(g => g.GroupId == student.GroupId)?.Name;

        var vm = new StudentDeleteViewModel
        {
            StudentId = student.StudentId,
            FirstName = student.FirstName,
            LastName = student.LastName,
            GroupId = student.GroupId,
            GroupName = groupName!
        };

        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _studentService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task<IEnumerable<SelectListItem>> GetGroupsSelectListAsync()
    {
        var groups = await _groupService.GetAllAsync();
        return groups.Select(g => new SelectListItem
        {
            Value = g.GroupId.ToString(),
            Text = g.Name
        });
    }
}
