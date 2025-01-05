using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IDepartmentService
    {
        void CreateDepartment(DepartmentDTO departmentDto);
        IEnumerable<DepDTO> GetAllDepartments();
        void UpdateDepartment(int departmentId, DepDTO departmentDto);
        void SetDepartmentActiveStatus(int departmentId, bool isActive);
        Department GetDepartmentByName(string department);
    }
}