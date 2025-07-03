using MyMvcApp.Application.DTOs;

namespace MyMvcApp.Application.Services.Groups;

public interface IGroupService
{
    Task<int> CreateAsync(GroupDto groupDto);
    Task<GroupDto?> GetAsync(int id);
    Task<IReadOnlyCollection<GroupDto>> GetAllAsync();
    Task<IReadOnlyCollection<GroupDto>> GetByCourseIdAsync(int courseId);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(GroupDto groupDto);
}
