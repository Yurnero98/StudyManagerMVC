using MyMvcApp.Domain.Entities;

namespace MyMvcApp.Domain.Repositories;

public interface IGroupRepository
{
    Task<int> CreateAsync(Group group);
    Task<Group?> GetAsync(int id);
    Task<IReadOnlyCollection<Group>> GetAllAsync();
    Task<IReadOnlyCollection<Group>> GetByCourseIdAsync(int courseId);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsExistAsync(string name);
    Task<bool> IsExistAsync(string name, int excludeGroupId);
    Task<bool> UpdateAsync(Group group);
}
