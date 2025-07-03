using AutoMapper;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Domain.Entities;
using MyMvcApp.Domain.Repositories;

namespace MyMvcApp.Application.Services.Courses;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository courseRepository, IMapper mapper, IGroupRepository groupRepository)
    {
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
        _mapper = mapper;

    }

    public async Task<int> CreateAsync(CourseDto courseDto)
    {
        if (await _courseRepository.IsExistAsync(courseDto.Name))
        {
            throw new InvalidDataException("Course with this name already exists.");
        }

        var course = _mapper.Map<Course>(courseDto);

        return await _courseRepository.CreateAsync(course).ConfigureAwait(false);
    }

    public async Task<CourseDto?> GetAsync(int id)
    {
        var course = await _courseRepository.GetAsync(id).ConfigureAwait(false);

        if (course == null)
            return null;

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<IReadOnlyCollection<CourseDto>> GetAllAsync()
    {
        var courses = await _courseRepository.GetAllAsync().ConfigureAwait(false);

        return _mapper.Map<IReadOnlyCollection<CourseDto>>(courses);
    }

    public async Task<bool> UpdateAsync(CourseDto courseDto)
    {
        if (await _courseRepository.IsExistAsync(courseDto.Name, courseDto.CourseId))
        {
            throw new InvalidDataException("Course with this name already exists.");
        }

        var course = _mapper.Map<Course>(courseDto);

        return await _courseRepository.UpdateAsync(course);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var groups = await _groupRepository.GetByCourseIdAsync(id);
        if (groups.Any())
        {
            throw new InvalidOperationException("Cannot delete course that has assigned groups.");
        }

        return await _courseRepository.DeleteAsync(id);
    }
}
