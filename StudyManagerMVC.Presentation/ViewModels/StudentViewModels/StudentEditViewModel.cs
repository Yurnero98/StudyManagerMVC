using Microsoft.AspNetCore.Mvc.Rendering;
using MyMvcApp.Application.DTOs;

namespace MyMvcApp.Presentation.ViewModels.StudentViewModels;

public class StudentEditViewModel
{
    public StudentDto? Student { get; set; }
    public IEnumerable<SelectListItem>? Groups { get; set; }
}
