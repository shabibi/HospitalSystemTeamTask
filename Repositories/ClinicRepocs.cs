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
            if (string.IsNullOrEmpty(branchName))
            {
                throw new ArgumentException("Branch name is required.");
            }

            return _context.Clinics
                .Join(
                    _context.Branches,
                    clinic => clinic.BID,
                    branch => branch.BID,
                    (clinic, branch) => new { Clinic = clinic, Branch = branch }
                )
                .Where(cb => cb.Branch.BranchName.ToLower() == branchName.ToLower())
                .Select(cb => cb.Clinic)
                .ToList();
        }

        public IEnumerable<Clinic> GetClinicsByDepartmentID(int depId)
        {
            if (depId <= 0)
            {
                throw new ArgumentException("Department ID must be greater than 0.");
            }

            return _context.Clinics
                .Where(clinic => clinic.DepID == depId)
                .ToList();
        }

        public void UpdateClinic(Clinic clinic)
        {
            _context.Clinics.Update(clinic);
            _context.SaveChanges();
        }

        public string GetClinicName(int cid)
        {
            try
            {
                var clinc = GetClinicById(cid);

                return clinc.ClincName;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

    }
}
