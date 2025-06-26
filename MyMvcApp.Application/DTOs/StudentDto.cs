namespace MyMvcApp.Application.DTOs;

public class StudentDto
{
    public int StudentId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public int GroupId { get; set; }
}

