using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IPatientRecordRepository
    {
        IEnumerable<PatientRecord> GetAll();
        PatientRecord GetById(int id);
        void Add(PatientRecord record);
        void UpdateRecord(PatientRecord record);
        void DeleteRecord(PatientRecord record);
    }
}