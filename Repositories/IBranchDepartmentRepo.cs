using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IBranchDepartmentRepo
    {
        void AddDepartmentToBranch(BranchDepartment branchDepartment);
        IEnumerable<Department> GetDepartmentsByBranch(int BranchID);
        IEnumerable<Branch> GetBranchByDepartments(int departmentId);
    }
}