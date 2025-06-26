using MyMvcApp.Application.DTOs;

namespace MyMvcApp.Application.Services.Courses;

public interface ICourseService
{
    Task<int> CreateAsync(CourseDto courseDto);
    Task<CourseDto?> GetAsync(int id);
    Task<IReadOnlyCollection<CourseDto>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(CourseDto courseDto);
}
