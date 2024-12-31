using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IBranchDepartmentRepo
    {
        void AddDepartmentToBranch(BranchDepartment branchDepartment);
    }
}