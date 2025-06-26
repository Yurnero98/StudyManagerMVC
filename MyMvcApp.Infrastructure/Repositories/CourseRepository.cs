using MyMvcApp.Domain.Entities;
using MyMvcApp.Domain.Repositories;
using MyMvcApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MyMvcApp.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course.CourseId;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var affected = await _context.Courses
            .Where(c => c.CourseId == id)
            .ExecuteDeleteAsync();

        return affected > 0;
    }

    public async Task<IReadOnlyCollection<Course>> GetAllAsync()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<Course?> GetAsync(int id)
    {
        return await _context.Courses.SingleOrDefaultAsync(x => x.CourseId == id);
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        var affected = await _context.Courses
            .Where(c => c.CourseId == course.CourseId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.Name, course.Name)
                .SetProperty(c => c.Description, course.Description)
            );

        return affected > 0;
    }

    public Task<bool> IsExistAsync(string name)
    {
        return _context.Courses.AnyAsync(c => c.Name == name);
    }

    public Task<bool> IsExistAsync(string name, int excludeCourseId)
    {
        return _context.Courses.AnyAsync(c => c.Name == name && c.CourseId != excludeCourseId);
    }
}
