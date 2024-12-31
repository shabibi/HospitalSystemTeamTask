using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IPatientRecordService
    {
        IEnumerable<PatientRecord> GetAllRecords();
        PatientRecord GetRecordById(int id);
        void CreateRecord(PatientRecord record);
        void UpdateRecord(PatientRecord record);
        void DeleteRecord(PatientRecord record);
        IEnumerable<PatientRecord> GetRecordsByDoctorId(int doctorId);

    }
}