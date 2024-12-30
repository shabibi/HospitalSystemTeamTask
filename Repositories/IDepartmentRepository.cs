using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IDepartmentRepository
    {
        void AddDepartment(Department department);
        void SaveChanges();
    }
}