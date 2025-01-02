using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IPatientService
    {
        IEnumerable<Patient> GetAllPatients();
        Patient GetPatientById(int Pid);
        void UpdatePatientDetails(int UID, PatientUpdate patientInput);
        void AddPatient(PatientInputDTO patientInput);
        Patient GetPatientByName(string PatientName);
        PatienoutputDTO GetPatientData(string? userName, int? Pid);

    }
}