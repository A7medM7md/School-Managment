using School.Core.Features.Departments.Queries.Responses;
using School.Data.Commons;
using School.Data.Entities;

namespace School.Core.Mapping.Departments
{
    public partial class DepartmentProfile
    {
        public void GetDepartmentByIdMapping()
        {
            CreateMap<Department, GetDepartmentByIdResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalizedName()))
                .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager.GetLocalizedName()))
                /// For Lists [Inline] -----
                ///.ForMember(dest => dest.Instructors,
                ///opt => opt.MapFrom(src => src.Instructors.Select(i => new InstructorResponse
                ///{
                ///    Id = i.Id,
                ///    Name = i.GetLocalizedName()
                ///})))
                ///.ForMember(dest => dest.Subjects,
                ///opt => opt.MapFrom(src => src.DepartmentSubjects.Select(ds => new SubjectResponse
                ///{
                ///    Id = ds.Subject.Id,
                ///    Name = ds.Subject.GetLocalizedName()
                ///})))
                ///.ForMember(dest => dest.Students,
                ///opt => opt.MapFrom(src => src.Students.Select(s => new StudentResponse
                ///{
                ///    Id = s.Id,
                ///    Name = s.GetLocalizedName()
                ///})));
                ///--------------------------
                /// Not Necessary For Students and Instructors List [Mapped Automatically]
                ///.ForMember(dest => dest.Instructors, opt => opt.MapFrom(src => src.Instructors))
                ///.ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students))
                .ForMember(dest => dest.Students, opt => opt.Ignore())
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.DepartmentSubjects));


            //CreateMap<Student, StudentResponse>()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalizedName()));

            CreateMap<DepartmentSubject, SubjectResponse>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Subject.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Subject.GetLocalizedName()));

            CreateMap<Instructor, InstructorResponse>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GetLocalizedName()));

        }
    }
}
