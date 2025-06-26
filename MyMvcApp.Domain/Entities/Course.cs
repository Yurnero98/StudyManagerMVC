namespace MyMvcApp.Domain.Entities;

public class Course
{
    public int CourseId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
