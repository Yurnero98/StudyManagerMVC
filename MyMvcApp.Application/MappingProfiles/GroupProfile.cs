using AutoMapper;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Domain.Entities;

namespace MyMvcApp.Application.MappingProfiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDto>().ReverseMap();
        }
    }
}