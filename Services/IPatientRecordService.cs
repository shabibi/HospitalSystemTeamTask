using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IPatientRecordService
    {
        IEnumerable<PatientRecordOutput> GetAllRecords();
        void CreateRecord(PatientRecordInputDTO record, int doctorId);
        void UpdateRecord(PatientRecord record);
        void DeleteRecord(PatientRecord record);
        IEnumerable<PatientRecordOutput> GetRecords(int? RecordId, int? patientId, int? doctorId, int? branchId);
    }
}