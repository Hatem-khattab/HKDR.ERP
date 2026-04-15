using HKDR.Common.DTOs.HR.Department;
using HKDR.DomainEntities.Entities;
using HKDR.Repository.IRepository;

namespace HKDR.API.Services.HR
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        // ========================
        // Get All
        // ========================
        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            var departments = await _repository.GetAllAsync();

            return departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();
        }

        // ========================
        // Create
        // ========================
        public async Task<int> CreateAsync(CreateDepartmentDto dto)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Department name is required");

            var exists = await _repository.ExistsAsync(d => d.Name == dto.Name);
            if (exists)
                throw new InvalidOperationException("Department already exists");

            var department = new Department
            {
                Name = dto.Name.Trim()
            };

            await _repository.AddAsync(department);
            await _repository.SaveAsync();

            return department.Id;
        }
    }
}
