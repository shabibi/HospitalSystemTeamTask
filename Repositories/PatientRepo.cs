
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using HospitalSystemTeamTask;

namespace HospitalSystemTeamTask.Repositories
{
    public class PatientRepo : IPatientRepo
    {
        public ApplicationDbContext _context;
        public PatientRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get All Patients
        public IEnumerable<Patient> GetAllPatients()
        {
            try
            {
                return _context.Patients.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        //Get Patients by id
        public Patient GetPatientsById(int Pid)
        {
            try
            {
                return _context.Patients.FirstOrDefault(u => u.PID == Pid);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public void UpdatePatient(Patient patient)
        {
            try
            {
                _context.Patients.Update(patient);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }


        }

        public void AddPatient(Patient patient)
        {
            try
            {
          
                // Add the Patient entity
                _context.Patients.Add(patient);

                // Save changes to the database
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

    }
}
