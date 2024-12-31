using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IBranchDepartmentService
    {
        void AddDepartmentToBranch(BranchDepDTO department);
    }
}