using AutoMapper;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Domain.Entities;
using MyMvcApp.Domain.Repositories;

namespace MyMvcApp.Application.Services.Groups;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;
    private readonly IStudentRepository _studentRepository;

    public GroupService(IGroupRepository groupRepository, IMapper mapper, IStudentRepository studentRepository)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
        _studentRepository = studentRepository;
    }

    public async Task<int> CreateAsync(GroupDto groupDto)
    {
        if (await _groupRepository.IsExistAsync(groupDto.Name))
        {
            throw new InvalidDataException("Group with this name already exists.");
        }

        var group = _mapper.Map<Group>(groupDto);
        return await _groupRepository.CreateAsync(group).ConfigureAwait(false);
    }

    public async Task<GroupDto?> GetAsync(int id)
    {
        var group = await _groupRepository.GetAsync(id).ConfigureAwait(false);
        if (group == null)
            return null;

        return _mapper.Map<GroupDto>(group);
    }

    public async Task<IReadOnlyCollection<GroupDto>> GetAllAsync()
    {
        var groups = await _groupRepository.GetAllAsync().ConfigureAwait(false);
        return _mapper.Map<IReadOnlyCollection<GroupDto>>(groups);
    }

    public async Task<IReadOnlyCollection<GroupDto>> GetByCourseIdAsync(int courseId)
    {
        var groups = await _groupRepository.GetByCourseIdAsync(courseId);
        return _mapper.Map<IReadOnlyCollection<GroupDto>>(groups);
    }

    public async Task<bool> UpdateAsync(GroupDto groupDto)
    {
        if (await _groupRepository.IsExistAsync(groupDto.Name, groupDto.GroupId))
        {
            throw new InvalidDataException("Group with this name already exists.");
        }

        var group = _mapper.Map<Group>(groupDto);
        return await _groupRepository.UpdateAsync(group);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var students = await _studentRepository.GetByGroupIdAsync(id);
        if (students.Any())
        {
            throw new InvalidOperationException("Cannot delete group that has assigned students.");
        }

        return await _groupRepository.DeleteAsync(id);
    }
}
