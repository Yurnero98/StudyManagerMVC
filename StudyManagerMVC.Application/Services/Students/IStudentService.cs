using MyMvcApp.Application.DTOs;

namespace MyMvcApp.Application.Services.Students;

public interface IStudentService
{
    Task<int> CreateAsync(StudentDto studentDto);
    Task<StudentDto?> GetAsync(int id);
    Task<IReadOnlyCollection<StudentDto>> GetAllAsync();
    Task<IReadOnlyCollection<StudentDto>> GetByGroupIdAsync(int groupId);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(StudentDto studentDto);
}
