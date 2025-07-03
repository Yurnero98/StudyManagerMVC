using Microsoft.AspNetCore.Mvc.Rendering;
using MyMvcApp.Application.DTOs;

namespace MyMvcApp.Presentation.ViewModels.GroupViewModels;

public class GroupEditViewModel
{
    public GroupDto? Group { get; set; }

    public IEnumerable<SelectListItem>? Courses { get; set; }
}
