using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAllPatients();
        Patient GetPatientById(int Pid);
        void UpdatePatientDetails(Patient updatedPatient);
        void AddPatient(PatientInputDTO patientInput);
    }
}
