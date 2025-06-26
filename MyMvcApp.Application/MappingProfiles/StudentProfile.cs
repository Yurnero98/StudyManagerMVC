using AutoMapper;
using MyMvcApp.Application.DTOs;
using MyMvcApp.Domain.Entities;

namespace MyMvcApp.Application.MappingProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
        }
    }
}