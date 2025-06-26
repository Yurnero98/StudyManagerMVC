using MyMvcApp.Domain.Entities;
using MyMvcApp.Domain.Repositories;
using MyMvcApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MyMvcApp.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _context;

    public GroupRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(Group group)
    {
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        return group.GroupId;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var affected = await _context.Groups
            .Where(g => g.GroupId == id)
            .ExecuteDeleteAsync();

        return affected > 0;
    }

    public async Task<IReadOnlyCollection<Group>> GetAllAsync()
    {
        return await _context.Groups.ToListAsync();
    }

    public async Task<Group?> GetAsync(int id)
    {
        return await _context.Groups.SingleOrDefaultAsync(x => x.GroupId == id);
    }

    public async Task<IReadOnlyCollection<Group>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Groups
            .Where(x => x.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Group group)
    {
        var affected = await _context.Groups
            .Where(g => g.GroupId == group.GroupId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(g => g.Name, group.Name)
                .SetProperty(g => g.CourseId, group.CourseId));

        return affected > 0;
    }

    public async Task<bool> IsExistAsync(string name)
    {
        return await _context.Groups.AnyAsync(x => x.Name == name);
    }

    public Task<bool> IsExistAsync(string name, int excludeGroupId)
    {
        return _context.Groups.AnyAsync(c => c.Name == name && c.GroupId != excludeGroupId);
    }
}
