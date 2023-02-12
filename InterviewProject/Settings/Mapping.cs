using AutoMapper;
using InterviewProject.Dtos;
using InterviewProject.Entities;

namespace InterviewProject.Settings
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, CreateStudentDto>().ReverseMap();
            CreateMap<Student, UpdateStudentDto>().ReverseMap();
            CreateMap<CourseStudent, CourseStudentDto>().ReverseMap();
            CreateMap<CourseStudent, RegisterCourseDto>().ReverseMap();
        }
    }
}
