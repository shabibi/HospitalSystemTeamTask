using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IDepartmentRepository
    {
        void AddDepartment(Department department);
        IEnumerable<Department> GetAllDepartments();
        void SaveChanges();
        Department GetDepartmentByName(string departmentName);
        void UpdateDepartment(int departmentId, Department updatedDepartment);
        void SetDepartmentActiveStatus(int departmentId, bool isActive);
    }
}