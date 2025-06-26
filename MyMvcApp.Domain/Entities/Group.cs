namespace MyMvcApp.Domain.Entities;

public class Group
{
    public int GroupId { get; set; }
    public required string Name { get; set; }
    public int CourseId { get; set; }
}
