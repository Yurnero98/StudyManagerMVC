namespace MyMvcApp.Presentation.ViewModels.GroupViewModels;

public class GroupDeleteViewModel
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = null!;

    public int CourseId { get; set; }
    public string CourseName { get; set; } = null!;
}
