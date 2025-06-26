using MyMvcApp.Domain.Entities;
using MyMvcApp.Domain.Repositories;
using MyMvcApp.Infrastructure.Data; 
using Microsoft.EntityFrameworkCore;

namespace MyMvcApp.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student.StudentId;
    }

    public async Task<bool> DeleteAsync(int studentId)
    {
        var affected = await _context.Students
            .Where(s => s.StudentId == studentId)
            .ExecuteDeleteAsync();

        return affected > 0;
    }

    public async Task<IReadOnlyCollection<Student>> GetAllAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student?> GetAsync(int id)
    {
        return await _context.Students.SingleOrDefaultAsync(x => x.StudentId == id);
    }

    public async Task<IReadOnlyCollection<Student>> GetByGroupIdAsync(int groupId)
    {
        return await _context.Students
            .Where(x => x.GroupId == groupId)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        var affected = await _context.Students
            .Where(s => s.StudentId == student.StudentId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.FirstName, student.FirstName)
                .SetProperty(s => s.LastName, student.LastName)
                .SetProperty(s => s.GroupId, student.GroupId));

        return affected > 0;
    }

    public async Task<bool> IsExistAsync(string firstName, string lastName)
    {
        return await _context.Students
            .AnyAsync(x => x.FirstName == firstName && x.LastName == lastName);
    }
}
