using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Infrastructure.Abstracts;
using School.Infrastructure.Abstracts.Functions;
using School.Service.Abstracts;

namespace School.Service.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorFunctionsRepository _instructorFunctionsRepo;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IFileService _fileService;

        public InstructorService(IInstructorFunctionsRepository instructorFunctionsRepo,
            IInstructorRepository instructorRepository,
            IFileService fileService)
        {
            _instructorFunctionsRepo = instructorFunctionsRepo;
            _instructorRepository = instructorRepository;
            _fileService = fileService;
        }

        public async Task<decimal> GetTotalSalary()
        {
            return await _instructorFunctionsRepo.GetInstructorsSalarySummation();
        }

        public async Task<List<Instructor>> GetAllInstructors()
        {
            return await _instructorFunctionsRepo.GetInstructorsData();
        }

        public async Task<bool> IsNameExists(string name)
        {
            return await _instructorRepository
                .GetTableNoTracking()
                .AnyAsync(i => i.NameEn.Equals(name) || i.NameAr.Equals(name));
        }

        public async Task<string> AddInstructorAsync(Instructor instructor, IFormFile image)
        {
            if (instructor is null)
                throw new ArgumentNullException(nameof(instructor));

            if (image is not null)
                instructor.Image = await _fileService.UploadImage("images/instructors", image);

            var result = await _instructorRepository.AddAsync(instructor);

            return result is null ? "Failed" : "Succeeded";
        }

    }
}
