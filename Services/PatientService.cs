
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;


namespace HospitalSystemTeamTask.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepo _PatientRepo;

        public PatientService(IPatientRepo PatientRepo)
        {
            _PatientRepo = PatientRepo;
        }



        public IEnumerable<Patient> GetAllPatients()
        {
            return _PatientRepo.GetAllPatients();

        }

        public Patient GetPatientById(int Pid)
        {
            var patient = _PatientRepo.GetPatientsById(Pid);
            if (patient == null)
                throw new KeyNotFoundException($"Patient with ID {Pid} not found.");
            return patient;
        }

        public void UpdatePatientDetails(Patient updatedPatient)
        {
            // Fetch the existing patient
            var existingPatient = _PatientRepo.GetPatientsById(updatedPatient.PID);

            if (existingPatient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            // Update patient-specific fields
            existingPatient.Age = updatedPatient.Age;
            existingPatient.Gender = updatedPatient.Gender;

            // Update user-specific fields if provided
            if (updatedPatient.User != null)
            {
                if (!string.IsNullOrEmpty(updatedPatient.User.UserName))
                {
                    existingPatient.User.UserName = updatedPatient.User.UserName;
                }

                if (!string.IsNullOrEmpty(updatedPatient.User.Email))
                {
                    existingPatient.User.Email = updatedPatient.User.Email;
                }

                if (!string.IsNullOrEmpty(updatedPatient.User.Password))
                {
                    existingPatient.User.Password = BCrypt.Net.BCrypt.HashPassword(updatedPatient.User.Password);
                }
            }

            // Save changes via the repository
            _PatientRepo.UpdatePatient(existingPatient);
        }
    }


}


