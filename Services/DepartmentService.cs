using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;

namespace HospitalSystemTeamTask.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public void CreateDepartment(DepartmentDTO departmentDto)
        {
            var department = new Department
            {
                DepartmentName = departmentDto.DepartmentName,
                Description = departmentDto.Description,
                IsActive = true // Default new departments to active
            };

            _departmentRepository.AddDepartment(department);
        }
    }
}
