namespace MyMvcApp.Application.DTOs;

public class CourseWithGroupsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<GroupDto> Groups { get; set; } = [];
}

