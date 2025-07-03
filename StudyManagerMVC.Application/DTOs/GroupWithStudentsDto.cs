namespace MyMvcApp.Application.DTOs;

public class GroupWithStudentsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<StudentDto> Students { get; set; } = [];
}

