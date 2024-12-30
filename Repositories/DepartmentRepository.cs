using HospitalSystemTeamTask.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystemTeamTask.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddDepartment(Department department)
        {
            _context.Departments.Add(department);
            SaveChanges();
        }
        public IEnumerable<Department> GetAllDepartments()
        {
            return _context.Departments.ToList();
        }

        public Department GetDepartmentByName(string departmentName)
        {
            return _context.Departments
                .Include(d => d.BranchDepartments)
                    .ThenInclude(bd => bd.Branch)
                .FirstOrDefault(d => d.DepartmentName.Equals(departmentName, StringComparison.OrdinalIgnoreCase));
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void UpdateDepartment(int departmentId, Department updatedDepartment)
        {
            var existingDepartment = _context.Departments.Find(departmentId);
            if (existingDepartment != null)
            {
                existingDepartment.DepartmentName = updatedDepartment.DepartmentName;
                existingDepartment.Description = updatedDepartment.Description;
                SaveChanges();
            }
        }

        public void SetDepartmentActiveStatus(int departmentId, bool isActive)
        {
            var department = _context.Departments.Find(departmentId);
            if (department != null)
            {
                department.IsActive = isActive;
                SaveChanges();
            }
        }
    }
}
