using AutoMapper;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Domain.Entities;

namespace MyMvcApp.Application.MappingProfiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
        }
    }
}
