using MyMvcApp.Application.DTOs;

namespace MyMvcApp.Presentation.ViewModels.GroupViewModels;

public class GroupIndexViewModel
{
    public IEnumerable<GroupDto> Groups { get; set; } = [];
    public Dictionary<int, string> Courses { get; set; } = [];
}
