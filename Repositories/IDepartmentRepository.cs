using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IDepartmentRepository
    {
        void AddDepartment(Department department);
        IEnumerable<Department> GetAllDepartments();
        void SaveChanges();
        Department GetDepartmentByName(string departmentName);
    }
}