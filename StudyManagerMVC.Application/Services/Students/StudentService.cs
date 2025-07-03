using AutoMapper;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Domain.Entities;
using MyMvcApp.Domain.Repositories;

namespace MyMvcApp.Application.Services.Students;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentService(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<int> CreateAsync(StudentDto studentDto)
    {
        var student = _mapper.Map<Student>(studentDto);
        return await _studentRepository.CreateAsync(student).ConfigureAwait(false);
    }

    public async Task<StudentDto?> GetAsync(int id)
    {
        var student = await _studentRepository.GetAsync(id).ConfigureAwait(false);
        if (student == null)
            return null;

        return _mapper.Map<StudentDto>(student);
    }

    public async Task<IReadOnlyCollection<StudentDto>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllAsync().ConfigureAwait(false);
        return _mapper.Map<IReadOnlyCollection<StudentDto>>(students);
    }

    public async Task<IReadOnlyCollection<StudentDto>> GetByGroupIdAsync(int groupId)
    {
        var students = await _studentRepository.GetByGroupIdAsync(groupId);
        return _mapper.Map<IReadOnlyCollection<StudentDto>>(students);
    }

    public async Task<bool> UpdateAsync(StudentDto studentDto)
    {
        var student = _mapper.Map<Student>(studentDto);
        return await _studentRepository.UpdateAsync(student);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _studentRepository.DeleteAsync(id);
    }
}
