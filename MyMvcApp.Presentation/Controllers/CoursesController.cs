using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Application.Services.Courses;

namespace MyMvcApp.Presentation.Controllers;

public class CoursesController : Controller
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IActionResult> Index()
    {
        var courses = await _courseService.GetAllAsync();
        return View(courses);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CourseDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _courseService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidDataException ex)
        {
            ModelState.AddModelError(nameof(dto.Name), ex.Message); 
            return View(dto);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var course = await _courseService.GetAsync(id);
        if (course is null) return NotFound();

        return View(course);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CourseDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);
        try
        {
            var updated = await _courseService.UpdateAsync(dto);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidDataException ex)
        {
            ModelState.AddModelError(nameof(dto.Name), ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var course = await _courseService.GetAsync(id);
        if (course is null) return NotFound();

        return View(course);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var deleted = await _courseService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            var course = await _courseService.GetAsync(id);
            if (course is null)
                return NotFound();

            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Delete", course);
        }
    }
}
