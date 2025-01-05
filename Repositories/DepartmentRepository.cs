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
            return _context.Departments.FirstOrDefault(d => d.DepartmentName == departmentName);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Department GetDepartmentById(int id)
        {
            return _context.Departments.FirstOrDefault(d => d.DepID == id);
        }

        public void UpdateDepartment(int id, Department updatedDepartment)
        {
            var existingDepartment = _context.Departments.FirstOrDefault(d => d.DepID == id);
            if (existingDepartment == null)
            {
                throw new KeyNotFoundException("Department not found.");
            }

            existingDepartment.DepartmentName = updatedDepartment.DepartmentName;
            existingDepartment.Description = updatedDepartment.Description;
            existingDepartment.IsActive = updatedDepartment.IsActive;

            _context.SaveChanges();
        }

        public void SetDepartmentActiveStatus(int departmentId, bool isActive)
        {
            var department = _context.Departments.Find(departmentId);
            if (department == null)
            {
                throw new KeyNotFoundException("Department not found.");
            }

            department.IsActive = isActive;
            SaveChanges();
        }

    }
}
