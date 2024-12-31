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
        public IEnumerable<DepartmentDTO> GetAllDepartments()
        {
            return _departmentRepository.GetAllDepartments()
                .Select(dept => new DepartmentDTO
                {
                    DepId = dept.DepID,
                    DepartmentName = dept.DepartmentName,
                    Description = dept.Description,
                    DepartmentStatus = dept.IsActive
                   
                }).ToList();
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

        public void UpdateDepartment(int departmentId, DepartmentDTO departmentDto)
        {
            var updatedDepartment = new Department
            {
                DepartmentName = departmentDto.DepartmentName,
                Description = departmentDto.Description
            };

            _departmentRepository.UpdateDepartment(departmentId, updatedDepartment);
        }

        public void SetDepartmentActiveStatus(int departmentId, bool isActive)
        {
            _departmentRepository.SetDepartmentActiveStatus(departmentId, isActive);
        }

        public Department GetDepartmentByName(string department)
        {
           return _departmentRepository.GetDepartmentByName(department);
        }


    }
}
