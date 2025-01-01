using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask;
using HospitalSystemTeamTask.Repositories;
using Microsoft.EntityFrameworkCore;
namespace HospitalSystemTeamTask.Repositories
{
    public class DoctorRepo : IDoctorRepo
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            try
            {
                return _context.Doctors.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
       

        public Doctor GetDoctorById(int Did)
        {
            try
            {
                return _context.Doctors.FirstOrDefault(u => u.DID == Did);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public Doctor GetDoctorByEmail(string email)
        {
            try
            {
               
                var doctor = _context.Doctors
                    .Include(d => d.User) 
                    .FirstOrDefault(d => d.User.Email == email);

                return doctor;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public bool EmailExists(string email)
        {
            try
            {
                return _context.Users.Any(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public Doctor GetDoctorByName(string docName)
        {
            if (string.IsNullOrEmpty(docName))
            {
                throw new ArgumentException("Doctor name cannot be null or empty.", nameof(docName));
            }

            try
            {
                // Use Include to join User with Doctor and filter by UserName
                var doctor = _context.Doctors
                    .Include(d => d.User) // Join with User table
                    .FirstOrDefault(d => d.User.UserName == docName);

                return doctor;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while accessing the database.", ex);
            }
        }


        public void AddDoctor(Doctor doctor)
        {
            try
            {

                // Add the Patient entity
                _context.Doctors.Add(doctor);

                // Save changes to the database
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public IEnumerable<Doctor> GetDoctorByBranchName(string branchName)
        {
            if (string.IsNullOrEmpty(branchName))
            {
                throw new ArgumentException("Branch name is required.");
            }

            branchName = branchName.Trim(); // Normalize input

            var doctors = _context.Doctors
                .Include(d => d.Branch)
                .Where(d => EF.Functions.Like(d.Branch.BranchName, branchName)) // Case-insensitive matching
                .ToList();

            if (!doctors.Any())
            {
                Console.WriteLine("No doctors found for the branch name: " + branchName);
            }

            return doctors;
        }

        public IEnumerable<Doctor> GetDoctorsByDepartmentName(string departmentName)
        {
            return _context.Doctors
                .Where(doc => doc.Department.DepartmentName == departmentName)
                .ToList();
        }








    }
}

