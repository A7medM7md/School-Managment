using AutoMapper;
using School.Core.Features.Students.Queries.Responses;
using School.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void GetStudentsListMapping()
        {
            CreateMap<Student, GetStudentsListResponse>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DName));
        }

    }
}
