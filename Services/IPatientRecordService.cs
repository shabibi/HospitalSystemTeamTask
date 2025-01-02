using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IPatientRecordService
    {
        IEnumerable<PatientRecordOutput> GetAllRecords();
        PatientRecord GetRecordById(int id);
        void CreateRecord(PatientRecordInputDTO record, int doctorId);
        void UpdateRecord(PatientRecord record);
        void DeleteRecord(PatientRecord record);
        IEnumerable<PatientRecordOutput> GetRecordsByDoctorId(int doctorId);
        IEnumerable<PatientRecord> GetRecordsByBranchId(int branchId);

    }
}