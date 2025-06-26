namespace MyMvcApp.Application.DTOs;

public class CourseDto
{
    public int CourseId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
