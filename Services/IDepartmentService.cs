using HospitalSystemTeamTask.DTO_s;

namespace HospitalSystemTeamTask.Services
{
    public interface IDepartmentService
    {
        void CreateDepartment(DepartmentDTO departmentDto);
        IEnumerable<DepartmentDTO> GetAllDepartments();
        void UpdateDepartment(int departmentId, DepartmentDTO departmentDto);
        void SetDepartmentActiveStatus(int departmentId, bool isActive);
    }
}