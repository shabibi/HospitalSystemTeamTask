using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;

namespace HospitalSystemTeamTask.Services
{
    public class BranchDepartmentService : IBranchDepartmentService
    {
        private readonly IBranchDepartmentRepo _branchDepartmentRepo;
        private readonly IBranchService _branchService;
        private readonly IDepartmentService _departmentService;

        public BranchDepartmentService(IBranchDepartmentRepo branchDepartmentRepo, IBranchService branchService, IDepartmentService departmentService)
        {
            _branchDepartmentRepo = branchDepartmentRepo;
            _branchService = branchService;
            _departmentService = departmentService;
        }

        public void AddDepartmentToBranch(BranchDepDTO department)
        {
            var departments = _departmentService.GetAllDepartments();

            // Validate if the department exists and is active
            if (!departments.Any(d => d.DepId == department.DID || d.DepartmentStatus))

                throw new Exception("The specified department does not exist.");

            // Validate if the branch exists and is active
            var branchs = _branchService.GetAllBranches();
            if (!branchs.Any(d => d.BID == department.BID || d.IsActive))
                throw new Exception("The specified branch does not exist.");

            var newBranchDep = new BranchDepartment
            {
                DepID = department.DID,
                BID = department.BID,
                DepartmentCapacity = department.Capacity
            };
            // Add the department to the branch
            _branchDepartmentRepo.AddDepartmentToBranch(newBranchDep);

        }

        public IEnumerable<DepartmentDTO>GetDepartmentsByBranch(string BranchName)
        {
            var branch = _branchService.GetBranchDetailsByBranchName(BranchName);
            if (branch == null || !branch.IsActive)
                throw new Exception($"{BranchName} Not Found");

           var departments = _branchDepartmentRepo.GetDepartmentsByBranch(branch.BID);
            List<DepartmentDTO> result = new List<DepartmentDTO>();
            foreach (var department in departments)
            {
                result.Add(new DepartmentDTO {
                    DepartmentName = department.DepartmentName,
                    DepartmentStatus = department.IsActive,
                    DepId = department.DepID 
                });
            }
            return result;
        }

        public IEnumerable<Branch> GetBranchsByDepartment(string DepartmentName)
        {
            var department = _departmentService.GetDepartmentByName(DepartmentName);
            if (department == null || !department.IsActive)
                throw new Exception($"{DepartmentName} Not Found");

            return _branchDepartmentRepo.GetBranchByDepartments(department.DepID);
       
             
        }
        public void UpdateBranchDepartment(BranchDepartment branchDepartment)
        {
            _branchDepartmentRepo.UpdateBranchDepartment(branchDepartment);
        }
        public BranchDepartment GetBranchDep(int departmentId, int branchId)
        {
           return _branchDepartmentRepo.GetBranchDep(departmentId, branchId);
        }
    }
}
