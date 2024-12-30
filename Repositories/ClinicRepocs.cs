using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask;
using System.Security.Cryptography;

namespace HospitalSystemTeamTask.Repositories
{
    public class ClinicRepo : IClinicRepocs
    {
        public ApplicationDbContext _context;
        public ClinicRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Clinic> GetAllClinic()
        {
            try
            {
                return _context.Clinics.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public void AddClinic(Clinic clinic)
        {
            try
            {
                _context.Clinics.Add(clinic);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }


        public Clinic GetClinicById(int clinicId)
        {
            try
            {
                return _context.Clinics.FirstOrDefault(u => u.CID == clinicId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public Clinic GetClinicByName(string ClinicName)
        {
            try
            {
                return _context.Clinics.FirstOrDefault(u => u.ClincName == ClinicName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public IEnumerable<Clinic> GetClinicsByBranchName(string branchName)
        {
            try
            {
                return _context.Clinics
                    .Where(c => c.Branch.BranchName == branchName)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
    }
}