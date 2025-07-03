namespace MyMvcApp.Presentation.ViewModels.StudentViewModels;

public class StudentDeleteViewModel
{
    public int StudentId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public int GroupId { get; set; }
    public string GroupName { get; set; } = null!;
}

