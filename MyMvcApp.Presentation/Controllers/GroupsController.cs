using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyMvcApp.Application.Services.Courses;
using MyMvcApp.Application.Services.Groups;
using MyMvcApp.Presentation.ViewModels.GroupViewModels;

namespace MyMvcApp.Presentation.Controllers;

public class GroupsController : Controller
{
    private readonly IGroupService _groupService;
    private readonly ICourseService _courseService;

    public GroupsController(IGroupService groupService, ICourseService courseService)
    {
        _groupService = groupService;
        _courseService = courseService;
    }

    public async Task<IActionResult> Index()
    {
        var groups = await _groupService.GetAllAsync();
        var courses = await _courseService.GetAllAsync();

        var vm = new GroupIndexViewModel
        {
            Groups = groups,
            Courses = courses.ToDictionary(c => c.CourseId, c => c.Name)
        };

        return View(vm);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new GroupEditViewModel
        {
            Courses = await GetCoursesSelectListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupEditViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Courses = await GetCoursesSelectListAsync();
            return View(vm);
        }

        try
        {
            await _groupService.CreateAsync(vm.Group!);
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidDataException ex)
        {
            ModelState.AddModelError("Group.Name", ex.Message);

            vm.Courses = await GetCoursesSelectListAsync();
            return View(vm);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var group = await _groupService.GetAsync(id);
        if (group == null) return NotFound();

        var vm = new GroupEditViewModel
        {
            Group = group,
            Courses = await GetCoursesSelectListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(GroupEditViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Courses = await GetCoursesSelectListAsync();
            return View(vm);
        }

        try
        {
            var updated = await _groupService.UpdateAsync(vm.Group!);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidDataException ex)
        {
            ModelState.AddModelError("Group.Name", ex.Message);

            vm.Courses = await GetCoursesSelectListAsync();
            return View(vm);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var group = await _groupService.GetAsync(id);
        if (group == null) return NotFound();

        var course = await _courseService.GetAsync(group.CourseId);

        var vm = new GroupDeleteViewModel
        {
            GroupId = group.GroupId,
            GroupName = group.Name,
            CourseId = group.CourseId,
            CourseName = course!.Name
        };

        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(GroupDeleteViewModel vm)
    {
        try
        {
            var deleted = await _groupService.DeleteAsync(vm.GroupId);
            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Delete", vm); 
        }
    }

    private async Task<IEnumerable<SelectListItem>> GetCoursesSelectListAsync()
    {
        var courses = await _courseService.GetAllAsync();
        return courses.Select(c => new SelectListItem
        {
            Value = c.CourseId.ToString(),
            Text = c.Name
        });
    }
}
