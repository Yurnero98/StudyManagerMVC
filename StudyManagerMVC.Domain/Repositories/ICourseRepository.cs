using MyMvcApp.Domain.Entities;

namespace MyMvcApp.Domain.Repositories;

public interface ICourseRepository
{
    Task<int> CreateAsync(Course course);
    Task<Course?> GetAsync(int id);
    Task<IReadOnlyCollection<Course>> GetAllAsync();
    Task<bool> DeleteAsync(int id);
    Task<bool> IsExistAsync(string name);
    Task<bool> IsExistAsync(string name, int excludeCourseId);
    Task<bool> UpdateAsync(Course course);
}
