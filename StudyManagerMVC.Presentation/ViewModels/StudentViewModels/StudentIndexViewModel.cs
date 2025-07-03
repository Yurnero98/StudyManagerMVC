using MyMvcApp.Application.DTOs;

namespace MyMvcApp.Presentation.ViewModels.StudentViewModels;

public class StudentIndexViewModel
{
    public IEnumerable<StudentDto> Students { get; set; } = [];
    public Dictionary<int, string> Groups { get; set; } = [];
}
