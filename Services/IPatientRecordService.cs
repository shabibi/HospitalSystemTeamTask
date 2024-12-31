using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IPatientRecordService
    {
        IEnumerable<PatientRecord> GetAllRecords();
        PatientRecord GetRecordById(int id);
        void CreateRecord(PatientRecord record);
    }
}