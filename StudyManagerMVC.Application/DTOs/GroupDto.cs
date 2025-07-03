namespace MyMvcApp.Application.DTOs;

public class GroupDto
{
    public int GroupId { get; set; }
    public required string Name { get; set; }
    public int CourseId { get; set; }
}
