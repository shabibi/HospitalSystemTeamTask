using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IBranchDepartmentService
    {
        void AddDepartmentToBranch(BranchDepDTO department);
        IEnumerable<DepDTO> GetDepartmentsByBranch(string BranchName);
        IEnumerable<Branch> GetBranchsByDepartment(string DepartmentName);
        void UpdateBranchDepartment(BranchDepartment branchDepartment);
        BranchDepartment GetBranchDep(int departmentId, int branchId);

    }
}