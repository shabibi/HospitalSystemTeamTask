using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IBranchDepartmentService
    {
        void AddDepartmentToBranch(BranchDepDTO department);
        IEnumerable<DepartmentDTO> GetDepartmentsByBranch(string BranchName);
        IEnumerable<BranchDTO> GetBranchsByDepartment(string DepartmentName);


    }
}