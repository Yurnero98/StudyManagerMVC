using MyMvcApp.Domain.Entities;

namespace MyMvcApp.Domain.Repositories;

public interface IStudentRepository
{
    Task<int> CreateAsync(Student student);
    Task<Student?> GetAsync(int id);
    Task<IReadOnlyCollection<Student>> GetAllAsync();
    Task<IReadOnlyCollection<Student>> GetByGroupIdAsync(int groupId);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsExistAsync(string firstName, string lastName);
    Task<bool> UpdateAsync(Student student);
}

